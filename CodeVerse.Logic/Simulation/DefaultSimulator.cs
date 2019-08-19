using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeVerse.Common;
using CodeVerse.Common.Commands;
using CodeVerse.Common.data;

namespace CodeVerse.Logic.Simulation
{
    public class DefaultSimulator : Simulator
    {
        private int MaxShipsPerPlayer;

        public DefaultSimulator(int MaxShipsPerPlayer = 1)
        {
            this.MaxShipsPerPlayer = MaxShipsPerPlayer;
        }

        public override List<shipData> Simulate(List<PlayerCommand> input = null)
        {
            /*
             * 
             * Order should be:
             * 
             * shoot
             * 
             * add to Velocity gravity
             * add to Velocity: move forces
             * move by velocity
             * 
             * Shields
             * collisions
             * 
             * scan
             * 
             * spawn new ships ( so that they are not instantly on another radar)
             * 
             */

            HandleShooting(input);

            SimulateWorldForces();
            HandleMoveOrders(input);
            MoveUnitsByVeloctiy();

            HandleShieldOrders(input);

            HandleCollisions();

            var outdata = HandleScanCommands(input);

            HandleSpawns(input);

            GenerateOutputForAllShips(ref outdata);

            return outdata;

        }

        /// <summary>
        /// Adds user output to ships that did not scan in this tick,
        /// this is needed for other fields like HP, energy, etc
        /// </summary>
        /// <param name="outdata"></param>
        private void GenerateOutputForAllShips(ref List<shipData> outdata)
        {
            // check for ships not present in output list
            var outDataGuids = outdata.Select(q => q.ship);

            var unscannedShips = entities
                .Where(q => q is Ship)
                .Select(q => q as Ship)
                .Where(q => !outDataGuids.Contains(q.ID))
                .ToList();

            foreach (var ship in unscannedShips)
            {
                outdata.Add(ShipToData(ship));
            }
        }

        /// <summary>
        /// Takes Scan Commands and scans for each specified ship. 
        /// Scan content and shipdata as written to output
        /// </summary>
        /// <param name="input"></param>
        /// <returns>shipdata to return to ClientConnector</returns>
        private List<shipData> HandleScanCommands(List<PlayerCommand> input)
        {
            var outlist = new List<shipData>();

            if (input != null)
            {
                var ScanCommands = input.Where(q => q is ScanCommand).Select(q => q as ScanCommand);

                foreach (var order in ScanCommands)
                {
                    var scanningShipIndex = entities.FindIndex(q => q.ID == order.shipID);

                    if (scanningShipIndex != -1)
                    {
                        var scanningShip = entities[scanningShipIndex] as Ship;

                        if (order.shipID == scanningShip.Owner)
                        {
                            var shipdata = ShipToData(scanningShip);

                            var TempScanCircle = new Entity()
                            {
                                radius = 150f,
                                pos = scanningShip.pos
                            };

                            var collisions = TempScanCircle.CollidesWithMultiple(entities);

                            foreach (var coll in collisions)
                            {
                                shipdata.userData.ScanContent.Add(
                                    EntityToRadarBlip(scanningShip, coll)
                                    );
                            }

                            outlist.Add(shipdata);
                        }
                    }
                }
            }

            return outlist;
        }

        /// <summary>
        /// Converts as scanned Entity to a Radar Blip in local (ship) space
        /// </summary>
        /// <param name="scanningShip"></param>
        /// <param name="scannedEntity"></param>
        /// <returns></returns>
        private static RadarBlip EntityToRadarBlip(Ship scanningShip, Entity scannedEntity)
        {
            var blipper = new RadarBlip();
            blipper.size = scannedEntity.radius;
            blipper.mass = scannedEntity.mass;
            blipper.pos = scanningShip.pos - scannedEntity.pos;

            if (scannedEntity is Sun)
                blipper.kind = BlipType.Sun;
            else if (scannedEntity is Planet)
                blipper.kind = BlipType.Planet;
            else if (scannedEntity is Moon)
                blipper.kind = BlipType.Moon;
            else if (scannedEntity is Ship)
                blipper.kind = BlipType.Ship;
            else if (scannedEntity is Bullet)
                blipper.kind = BlipType.Bullet;

            return blipper;
        }

        /// <summary>
        /// Takes Spawn Commands and adds ships, if the maxShip limit (per user) is not reached
        /// </summary>
        /// <param name="input"></param>
        private void HandleSpawns(List<PlayerCommand> input)
        {
            if (input != null)
            {
                var SpawnOrders = input.Where(q => q is SpawnCommand).Select(q => q as SpawnCommand).ToList();

                Vector MapExtends = new Vector(0, 0);

                foreach (var item in entities)
                {
                    if (item.pos.X + item.radius > MapExtends.X)
                        MapExtends.X = item.pos.X + item.radius;

                    if (item.pos.Y + item.radius > MapExtends.Y)
                        MapExtends.Y = item.pos.Y + item.radius;
                }

                foreach (var order in SpawnOrders)
                {
                    // check if player already has enough ships
                    var playerShips = entities
                        .Where(q => q is Ship)
                        .Select(q => q as Ship)
                        .Where(q => q.Owner == order.playerID)
                        .ToList();

                    if (playerShips.Count < MaxShipsPerPlayer)
                    {
                        var newShip = new Ship(
                            ID: new Guid(),
                            name: order.playerID.ToString(),
                            owner: order.playerID,
                            hP: 100,
                            energy: 100,
                            pos: StaticRandom.RandomVecInCircle(),
                            velocity: Vector.Zero
                            );

                        while (newShip.CollidesWithMultiple(entities).Count != 0)
                            newShip.pos = StaticRandom.RandomVecInSquare(newShip.radius, MapExtends.Length - newShip.radius);

                        entities.Add(newShip);
                    }
                }
            }
        }

        /// <summary>
        /// Converts as Ship to sendable ship data containing the appropriate fields
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private shipData ShipToData(Ship ship)
        {
            var userOutData = new UserShipData()
            {
                name = ship.name,
                Energy = ship.Energy,
                Mass = ship.mass,
                ScanContent = new List<RadarBlip>()
            };

            var shipOutData = new shipData()
            {
                player = ship.Owner,
                //universe = ship.UniverseID,
                ship = ship.Owner,
                userData = userOutData
            };

            return shipOutData;
        }

        /// <summary>
        /// Checks for collisions between each entity with all the other entities,
        /// taking shielding into account.
        /// Colliding non-static entitties are destroyed
        /// </summary>
        private void HandleCollisions()
        {
            var Movables = entities
                .Where(q => q is MovingEntity)
                .Select(q => q as MovingEntity)
                .ToList();

            var CollidedMovables = new List<Entity>();
            foreach (var movable in Movables)
            {
                var collisions = movable.CollidesWithMultiple(entities);

                if (collisions.Count > 0)
                {
                    var other = collisions[0];

                    if (movable is Ship && ((Ship)movable).shield && (other is Ship || other is Bullet))
                    {
                        // shield successfully blocked a shot or another player
                        Console.WriteLine(movable.name + " blocked colliosion with " + other.name);
                    }
                    else
                    {
                        CollidedMovables.Add(movable);

                        if (movable is Ship)
                            Console.WriteLine(movable.name + " died colliding with " + other.name);
                    }

                }
            }

            foreach (var item in CollidedMovables)
                entities.Remove(item);
        }

        /// <summary>
        /// Enables Shields for the next tick for the specified ships.
        /// Disables shields for ships not enabling it
        /// </summary>
        /// <param name="input"></param>
        private void HandleShieldOrders(List<PlayerCommand> input)
        {
            // first, disable all shields
            foreach (var ship in entities.Where(q => q is Ship).Select(q => q as Ship))
                ship.shield = false;

            if (input != null)
            {
                var ShieldOrders = input.Where(q => q is ShieldCommand).Select(q => q as ShieldCommand).ToList();

                foreach (var order in ShieldOrders)
                {
                    var shieldingShipIndex = entities.FindIndex(q => q.ID == order.shipID);

                    if (shieldingShipIndex != -1)
                    {
                        var shieldingShip = entities[shieldingShipIndex] as Ship;

                        if (shieldingShip.Owner == order.playerID)
                            shieldingShip.shield = true;
                    }
                }
            }
        }

        /// <summary>
        /// Moves all MovingEntities by the calculated velocity
        /// </summary>
        private void MoveUnitsByVeloctiy()
        {
            var Movables = entities
                .Where(q => q is MovingEntity)
                .Select(q => q as MovingEntity)
                .ToList();

            foreach (var movable in Movables)
            {
                if (movable.PositionHistory.Count > 250)
                    movable.PositionHistory.RemoveAt(0);

                movable.PositionHistory.Add(movable.pos);
                movable.pos += movable.Velocity;
            }
        }

        /// <summary>
        /// Spawns a Bullet from specified ships into specified direction with specified power,
        /// and adds it to entities
        /// </summary>
        /// <param name="input"></param>
        private void HandleShooting(List<PlayerCommand> input)
        {
            if (input != null)
            {
                var ShootOrders = input.Where(q => q is ShootCommand).Select(q => q as ShootCommand).ToList();
                var newBullets = new List<Bullet>();

                foreach (var order in ShootOrders)
                {
                    var shootingShipIndex = entities.FindIndex(q => q.ID == order.shipID);

                    if (shootingShipIndex != -1)
                    {
                        var shootingShip = entities[shootingShipIndex] as Ship;

                        if (shootingShip.Owner == order.playerID)
                        {
                            var newBullet = new Bullet(
                            name: shootingShip + "'s bullet",
                            originID: shootingShip.ID,
                            pos: shootingShip.pos,
                            vel: (order.Direction.Normalized * order.Power)
                            );

                            newBullets.Add(newBullet);
                        }
                    }
                }

                entities.AddRange(newBullets);
            }
        }

        /// <summary>
        /// Takes Move Order Input and adds it to the velocity of the specified Ships
        /// </summary>
        /// <param name="input"></param>
        private void HandleMoveOrders(List<PlayerCommand> input)
        {
            if (input != null)
            {
                var MoveOrders = input.Where(q => q is MoveCommand).Select(q => q as MoveCommand).ToList();

                foreach (var order in MoveOrders)
                {
                    var movingShipIndex = entities.FindIndex(q => q.ID == order.shipID);

                    if (movingShipIndex != -1)
                    {
                        var movingShip = entities[movingShipIndex] as Ship;

                        if (movingShip.Owner == order.playerID)
                            movingShip.Velocity += order.Force / movingShip.mass;
                    }
                }
            }
        }

        /// <summary>
        /// Calculates gravitational forces and adds it to the velocity of each MovingEntity
        /// For now, only Suns are considered for gravity
        /// </summary>
        private void SimulateWorldForces()
        {
            var Movables = entities
                .Where(q => q is MovingEntity)
                .Select(q => q as MovingEntity)
                .ToList();

            // was nehmen wir alles für Gravity? alles oder alles static oder nur sonnen?

            //var Gravitationals = entities
            //.Where(q => q is StaticEntity)
            //.ToList();

            var Gravitationals = entities
                .Where(q => q is Sun)
                .ToList();

            foreach (var movable in Movables)
            {
                foreach (var grav in Gravitationals)
                {
                    Vector localVec = Vector.VecFromTo(grav.pos, movable.pos);

                    // now for the real shit, real physics formula
                    // Force = Gravitational Constant * Mass1 * Mass2 / distance²
                    // Gravitational Constant is 6.67408 × 10^(-11) 
                    float GravConstant = 0.0000000000667408f;
                    GravConstant *= 1000000000f;
                    float appliedGravityPower = GravConstant * ((grav.mass * movable.mass) / (localVec.Length * localVec.Length));

                    var GravVector = localVec * appliedGravityPower;

                    movable.Velocity += GravVector / movable.mass;
                }
            }
        }
    }
}
