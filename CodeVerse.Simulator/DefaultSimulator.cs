using CodeVerse.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Simulator
{
    public class DefaultSimulator : ICanSimulate
    {
        private List<Entity> entities;
        private Random random = new Random();

        public List<Entity> GenerateMap()
        {
            entities = new List<Entity>();

            // eine Sonne auf jeden Fall
            entities.Add(RandomSun("Sun_start"));

            // generate a random map of static objects
            for (int i = 0; i < 20; i++)
            {
                int entityType = StaticExtensions.GetRandomWeightedIndex(1f, 5f, 10f);
                switch (entityType)
                {
                    case 0: entities.Add(RandomSun("Sun_" + i)); break;
                    case 1: entities.Add(RandomPlanet("Planet_" + i)); break;
                    case 2: entities.Add(RandomMoon("Moon_" + i)); break;
                    default: break;
                }
            }

            var ship = new Ship();
            ship.Velocity = new Vector();
            ship.name = "Ship_0";
            ship.radius = 5f;
            ship.Owner = "bob";
            ship.HP = 100;
            ship.Energy = 100;
            ship.Weight = 500f;
            ship.pos = new Vector(randomNormalizedFloat * 50f, randomNormalizedFloat * 50f);
            entities.Add(ship);

            var bullet = new Bullet();
            bullet.name = "Bullet_0";
            bullet.origin = ship.name;
            bullet.pos = new Vector();
            bullet.Velocity = new Vector(500, 100);
            bullet.radius = 1f;
            bullet.Weight = 50f;
            entities.Add(bullet);

            return entities;
        }

        private Sun RandomSun(string name)
        {
            var newObj = new Sun();
            newObj.pos = new Vector(randomNormalizedFloat * 50f, randomNormalizedFloat * 50f);
            newObj.name = name;
            newObj.radius = randomNormalizedFloat * 20f;
            newObj.Gravity = randomNormalizedFloat * 10f;
            return newObj;
        }

        private Planet RandomPlanet(string name)
        {
            var newObj = new Planet();
            newObj.pos = new Vector(randomNormalizedFloat * 50f, randomNormalizedFloat * 50f);
            newObj.name = name;
            newObj.radius = randomNormalizedFloat * 20f;
            newObj.Gravity = randomNormalizedFloat * 1f;
            return newObj;
        }

        private Moon RandomMoon(string name)
        {
            var newObj = new Moon();
            newObj.pos = new Vector(randomNormalizedFloat * 50f, randomNormalizedFloat * 50f);
            newObj.name = name;
            newObj.radius = randomNormalizedFloat * 20f;
            newObj.Gravity = 0f;
            return newObj;
        }

        public List<Entity> Simulate(List<Entity> input)
        {
            // handle user ticks here
            throw new NotImplementedException();
        }

        public List<Entity> Wipe()
        {
            // kill everything non-static
            throw new NotImplementedException();
        }

        private float randomNormalizedFloat { get { return Convert.ToSingle(random.NextDouble()); } }
    }
}
