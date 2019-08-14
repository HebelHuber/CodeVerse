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
            for (int i = 0; i < 20; i++)
            {
                var newObj = new StaticEntity();
                newObj.pos = new Vector(randomNormalizedFloat * 50f, randomNormalizedFloat * 50f);
                newObj.name = "looool";
                newObj.radius = randomNormalizedFloat * 20f;
                newObj.Gravity = randomNormalizedFloat * 5f;
                entities.Add(newObj);
            }

            return entities;
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
