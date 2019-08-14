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

            // generate a random map of static objects
            for (int i = 0; i < 5; i++)
                entities.Add(RandomSun("Sun_" + i));

            for (int i = 0; i < 10; i++)
                entities.Add(RandomPlanet("Planet_" + i));

            for (int i = 0; i < 20; i++)
                entities.Add(RandomMoon("Moon_" + i));

            var ship = new Ship();
            ship.Velocity = new Vector();
            ship.Owner = "bob";
            ship.HP = 100;
            ship.Energy = 100;

            entities.Add(ship);

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
