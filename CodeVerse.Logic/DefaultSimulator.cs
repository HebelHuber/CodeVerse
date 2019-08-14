using CodeVerse.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Logic
{
    public class DefaultSimulator : ICanSimulate
    {
        private List<Entity> entities;

        public List<Entity> GenerateMap(int seed = 0)
        {
            if (seed == 0)
                seed = new Random().Next(int.MaxValue);

            StaticExtensions.SetRandomSeed(seed);

            entities = new List<Entity>();

            // generate a random map of static objects

            // some Suns
            for (int i = 0; i < 2; i++)
                entities.Add(Sun.Random("Sun_" + i));

            // some Planets
            for (int i = 0; i < 5; i++)
                entities.Add(Planet.Random("Planet_" + i));

            // some Moons
            for (int i = 0; i < 10; i++)
                entities.Add(Moon.Random("Moon_" + i));

            // add a ship and a bullet
            entities.Add(Ship.Random("Bob", "Ship_0"));
            entities.Add(Bullet.Random("Ship_0"));

            return entities;
        }

        public List<Entity> Simulate(List<PlayerCommand> input)
        {
            // handle user ticks here
            throw new NotImplementedException();
        }

        public List<Entity> Wipe()
        {
            // kill everything non-static
            throw new NotImplementedException();
        }
    }
}
