using CodeVerse.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeVerse.Logic.Maps
{
    public class RandomMap : IMapGenerator
    {
        private float mapsize;
        private int seed;

        public RandomMap(float mapsize = 1000f, int seed = 0)
        {
            this.mapsize = mapsize;
            this.seed = seed;
        }

        public List<Entity> Generate()
        {
            Console.Write("Generating map...");

            if (seed == 0)
                seed = new Random().Next(int.MaxValue);

            StaticRandom.SetSeed(seed);

            var entitycountmult = Math.Max(1, Convert.ToInt32(mapsize / 1000f));
            var entities = new List<Entity>();

            // generate a random map

            // some Suns
            int SunCount = StaticRandom.randomInt(1 * entitycountmult, 2 * entitycountmult);
            for (int i = 0; i < SunCount; i++)
            {
                var e = Sun.Random("Sun_" + i, mapsize);

                while (e.CollidesWithMultiple(entities).Count != 0)
                    e.pos = StaticRandom.RandomVecInSquare(e.radius, mapsize - e.radius);

                entities.Add(e);
            }

            // some Planets
            int PlanetCount = StaticRandom.randomInt(1 * entitycountmult, 5 * entitycountmult);
            for (int i = 0; i < PlanetCount; i++)
            {
                var e = Planet.Random("Planet_" + i, mapsize);

                while (e.CollidesWithMultiple(entities).Count != 0)
                    e.pos = StaticRandom.RandomVecInSquare(e.radius, mapsize - e.radius);

                entities.Add(e);
            }

            // some Moons
            int MoonCount = StaticRandom.randomInt(1 * entitycountmult, 10 * entitycountmult);
            for (int i = 0; i < MoonCount; i++)
            {
                var e = Moon.Random("Moon_" + i, mapsize);

                while (e.CollidesWithMultiple(entities).Count != 0)
                    e.pos = StaticRandom.RandomVecInSquare(e.radius, mapsize - e.radius);

                entities.Add(e);
            }

            // some ships
            int ShipCount = StaticRandom.randomInt(5 * entitycountmult, 20 * entitycountmult);
            for (int i = 0; i < ShipCount; i++)
            {
                var ship = Ship.Random(new Guid(), new Guid(), "Ship_" + i, mapsize);
                while (ship.CollidesWithMultiple(entities).Count != 0)
                {
                    ship.pos = StaticRandom.RandomVecInSquare(ship.radius, mapsize - ship.radius);
                }
                entities.Add(ship);
            }

            var ships = entities.Where(q => q is Ship).Select(q => q as Ship).ToList();

            // some bullets
            foreach (var ship in ships)
            {
                int bulletsPerShip = 4;
                for (int i = 0; i < bulletsPerShip; i++)
                {
                    Vector vel = Vector.FromAngleLength(i * (360f / bulletsPerShip), 5f);
                    var bullet = new Bullet("blt", ship.ID, ship.pos + vel, ship.Velocity + vel);
                    bullet.name = ship.name + "'s bullet";
                    entities.Add(bullet);
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Done (" + entities.Count + "entities)");
            Console.ResetColor();

            return entities;
        }
    }
}


