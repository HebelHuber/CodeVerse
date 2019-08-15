using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeVerse.Common;

namespace CodeVerse.Logic
{
    public class DefaultSimulator : Simulator
    {
        public DefaultSimulator(int seed, float mapsize, float GravityMultiplier = 1, bool debugmode = false)
        {
            base.Init(seed, mapsize, GravityMultiplier, debugmode);
        }

        public override void MapGen()
        {
            // here we have an example of a hardcoded map for a specific seed
            if (seed == -1)
            {
                float leftX = (mapsize / 4f);
                float rightX = (mapsize / 4f) * 3f;
                float upperY = (mapsize / 4f);
                float lowerY = (mapsize / 4f) * 3f;
                entities.Add(new Sun("Sun_UL_10", 10f, new Vector(leftX, upperY)));
                entities.Add(new Sun("Sun_UR_50", 50f, new Vector(rightX, upperY)));
                entities.Add(new Sun("Sun_LL_75", 75f, new Vector(leftX, lowerY)));
                entities.Add(new Sun("Sun_LR_100", 100f, new Vector(rightX, lowerY)));

                Vector center = new Vector((mapsize / 4f) * 2f, (mapsize / 4f) * 2f);
                entities.Add(new Ship("Bob", "Lukas", 100, 100, center, Vector.Zero));

                return;
            }
            else if (seed == -2)
            {
                Vector center = new Vector((mapsize / 4f) * 2f, (mapsize / 4f) * 2f);
                var sun = new Sun("Sun_Main_1.0", 80f, center);
                entities.Add(sun);

                float dist = 1.2f;
                for (int i = 0; i < 10; i++)
                {
                    var shipPos = new Vector(center.X - (sun.radius * dist), center.Y);
                    entities.Add(new Ship(dist.ToString(), "Lukas", 100, 100, shipPos, new Vector(0, 1f + dist)));
                    dist += 0.5f;
                }

                return;
            }

            // generate a random map

            // some Suns
            int SunCount = StaticRandom.randomInt(2, 2);
            for (int i = 0; i < SunCount; i++)
            {
                var e = Sun.Random("Sun_" + i, mapsize);

                while (e.CollidesWithMultiple(entities).Count != 0)
                    e.pos = StaticRandom.RandomVecInSquare(e.radius, mapsize - e.radius);

                entities.Add(e);
            }

            // some Planets
            int PlanetCount = StaticRandom.randomInt(1, 5);
            for (int i = 0; i < PlanetCount; i++)
            {
                var e = Planet.Random("Planet_" + i, mapsize);

                while (e.CollidesWithMultiple(entities).Count != 0)
                    e.pos = StaticRandom.RandomVecInSquare(e.radius, mapsize - e.radius);

                entities.Add(e);
            }

            // some Moons
            int MoonCount = StaticRandom.randomInt(1, 10);
            for (int i = 0; i < MoonCount; i++)
            {
                var e = Moon.Random("Moon_" + i, mapsize);

                while (e.CollidesWithMultiple(entities).Count != 0)
                    e.pos = StaticRandom.RandomVecInSquare(e.radius, mapsize - e.radius);

                entities.Add(e);
            }

            // some ships
            int ShipCount = StaticRandom.randomInt(10, 20);
            for (int i = 0; i < ShipCount; i++)
            {
                var ship = Ship.Random("Bob", "Ship_" + i, mapsize);
                while (ship.CollidesWithMultiple(entities).Count != 0)
                {
                    ship.pos = StaticRandom.RandomVecInSquare(ship.radius, mapsize - ship.radius);
                }
                entities.Add(ship);
            }

            // some bullets
            int BulletCount = StaticRandom.randomInt(10, 50);
            for (int i = 0; i < BulletCount; i++)
            {
                var bullet = Bullet.Random("Ship_0", mapsize);
                while (bullet.CollidesWithMultiple(entities).Count != 0)
                    bullet.pos = StaticRandom.RandomVecInSquare(bullet.radius, mapsize - bullet.radius);
                entities.Add(bullet);
            }
        }

        protected override List<ScannerContent> SimulateInternal(List<PlayerCommand> input = null)
        {
            if (input != null)
            {
                // handle user ticks here
                foreach (var cmd in input)
                {
                    var targetIndex = entities.FindIndex(q => q.name == cmd.targetID);

                    if (targetIndex != -1)
                    {
                        Ship target = (Ship)entities[targetIndex];

                        if (cmd is MoveCommand)
                        {
                            MoveCommand parsedCmd = (MoveCommand)cmd;
                            target.Velocity += parsedCmd.Force;
                        }
                        if (cmd is ShootCommand)
                        {
                            ShootCommand parsedCmd = (ShootCommand)cmd;
                            var bullet = new Bullet();

                            string uniqueBulletName = target.name + "_bullet_";

                            var bulletsfromsameship = entities
                                .Where(q => q is Bullet)
                                .Select(q => q as Bullet)
                                .Where(q => q.origin == target.name)
                                .Select(q => q.name)
                                .ToList();

                            int indexer = 0;
                            while (bulletsfromsameship.Contains(uniqueBulletName + indexer.ToString()))
                                indexer++;

                            bullet.name = uniqueBulletName + indexer.ToString();

                            bullet.origin = target.name;
                            bullet.Velocity = target.Velocity + (parsedCmd.Direction * parsedCmd.Power);
                            bullet.pos = target.pos;
                            entities.Add(bullet);
                        }
                        if (cmd is ShieldCommand)
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
            }

            var Movables = entities
                .Where(q => q is MovingEntity)
                .Select(q => q as MovingEntity)
                .ToList();

            foreach (var movable in Movables)
                ApplyWorldForces(movable);

            foreach (var movable in Movables)
            {
                if (movable.PositionHistory.Count > 20)
                    movable.PositionHistory.RemoveAt(0);

                movable.PositionHistory.Add(movable.pos);
                movable.pos += movable.Velocity;
            }

            var CollidedMovables = new List<Entity>();
            foreach (var movable in Movables)
            {
                var collisions = movable.CollidesWithMultiple(entities);

                if (collisions.Count > 0)
                {
                    Console.WriteLine(movable.name + " died colliding with " + collisions[0].name);
                    CollidedMovables.Add(movable);
                }
            }

            foreach (var item in CollidedMovables)
                entities.Remove(item);

            if (CollidedMovables.Count != 0)
            {
                int moavblesleft = Movables.Count - CollidedMovables.Count;
                Console.WriteLine(moavblesleft + " movables still alive");
            }

            return new List<ScannerContent>();
        }

        private void ApplyWorldForces(MovingEntity unit)
        {
            // was nehmen wir alles für Gravity? alles oder alles static oder nur sonnen?

            //var Gravitationals = entities
            //    .Where(q => q != unit)
            //    .ToList();

            //var Gravitationals = entities
            //    .Where(q => q is StaticEntity && q != unit)
            //    .ToList();

            var Gravitationals = entities
                .Where(q => q is Sun && q != unit)
                .ToList();

            foreach (var grav in Gravitationals)
            {
                Vector localVec = Vector.VecFromTo(grav.pos, unit.pos);

                // now for the real shit, real physics formula
                // Force = Gravitational Constant * Mass1 * Mass2 / distance²
                // Gravitational Constant is 6.67408 × 10^(-11) 
                float GravConstant = 0.0000000000667408f;
                GravConstant *= 1000000000f;
                float appliedGravityPower = GravConstant * (grav.mass * unit.mass) / (localVec.Length * localVec.Length);

                var GravVector = localVec * appliedGravityPower * GravityMultiplier;

                unit.Velocity += GravVector;
            }
        }
    }
}
