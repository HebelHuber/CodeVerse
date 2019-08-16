using CodeVerse.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Logic.Maps
{
    public class CenterSun : IMapGenerator
    {
        private float mapsize;

        public CenterSun(float mapsize = 1000f)
        {
            this.mapsize = mapsize;
        }

        public List<Entity> Generate()
        {
            var entities = new List<Entity>();

            Vector center = new Vector((mapsize / 4f) * 2f, (mapsize / 4f) * 2f);
            var sun = new Sun("Sun_Main_1.0", 80f, center);
            entities.Add(sun);

            float dist = 1.2f;
            for (int i = 0; i < 10; i++)
            {
                var shipPos = new Vector(center.X - (sun.radius * dist), center.Y);
                entities.Add(new Ship(i, dist.ToString(), "Lukas", 100, 100, shipPos, new Vector(0, (dist * 0.5f))));
                dist += 0.5f;
            }

            return entities;
        }
    }
}
