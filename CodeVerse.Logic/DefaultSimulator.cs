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
                StaticRandom.SetRandomSeed();
            else
                StaticRandom.SetSeed(seed);

            entities = new List<Entity>();

            // generate a random map of static objects

            // some Suns
            int SunCount = StaticRandom.randomInt(1, 3);
            for (int i = 0; i < SunCount; i++)
                entities.Add(Sun.Random("Sun_" + i));

            // some Planets
            int PlanetCount = StaticRandom.randomInt(3, 10);
            for (int i = 0; i < PlanetCount; i++)
                entities.Add(Planet.Random("Planet_" + i));

            // some Moons
            int MoonCount = StaticRandom.randomInt(10, 20);
            for (int i = 0; i < MoonCount; i++)
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
