using CodeVerse.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeVerse.Logic
{
    public class DefaultSimulator : ICanSimulate
    {
        private List<Entity> entities;
        public List<Entity> GetDebugEntities()
        {
            if (EntitiesArePublic)
                return entities;
            else
                throw new Exception("Simulator not in debug mode, no access to entities.");
        }

        public bool IsDebugMode() { return EntitiesArePublic; }

        private bool EntitiesArePublic;

        public void GenerateMap(int seed = 0, bool PublicEntities = false)
        {
            this.EntitiesArePublic = PublicEntities;

            if (seed == 0)
                StaticRandom.SetRandomSeed();
            else
                StaticRandom.SetSeed(seed);

            entities = new List<Entity>();

            // generate a random map of static objects
            float mapsize = 500f;

            // some Suns
            int SunCount = StaticRandom.randomInt(1, 2);
            for (int i = 0; i < SunCount; i++)
                entities.Add(Sun.Random("Sun_" + i, mapsize));

            // some Planets
            int PlanetCount = StaticRandom.randomInt(3, 10);
            for (int i = 0; i < PlanetCount; i++)
                entities.Add(Planet.Random("Planet_" + i, mapsize));

            // some Moons
            int MoonCount = StaticRandom.randomInt(10, 20);
            for (int i = 0; i < MoonCount; i++)
                entities.Add(Moon.Random("Moon_" + i, mapsize));

            // add a ship and a bullet
            entities.Add(Ship.Random("Bob", "Ship_0", mapsize));
            entities.Add(Bullet.Random("Ship_0", mapsize));
        }

        public List<ScannerContent> Simulate(List<PlayerCommand> input = null)
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

            foreach (var item in Movables)
                ApplyWorldForces(item);

            foreach (var item in Movables)
                MoveFromVelocity(item);

            return new List<ScannerContent>();
        }

        private void ApplyWorldForces(MovingEntity unit)
        {
            var Gravitationals = entities
                .Where(q => q is StaticEntity)
                .Select(q => q as StaticEntity)
                .Where(q => q.Gravity > 0)
                .ToList();

            foreach (var grav in Gravitationals)
            {
                Vector offset = Vector.VecFromTo(grav.pos, unit.pos);
                float strength = grav.Gravity;

                // replace this later with "open end" method
                float appliedGravityPower = offset.Length.Remap(0, 5000, 1, 0, clamp: true) * strength;

                var GravVector = offset * appliedGravityPower * 0.001f;
                Console.WriteLine("Force from " + grav.name + " on " + unit.name + ": " + GravVector);

                unit.Velocity += GravVector;
            }
        }

        private void MoveFromVelocity(MovingEntity unit)
        {
            unit.pos += unit.Velocity;
        }
    }
}
