using CodeVerse.Common;
using CodeVerse.Logic.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Logic.Maps
{
    public class FourSuns : IMapGenerator
    {
        private float mapsize;

        public FourSuns(float mapsize = 1000f)
        {
            this.mapsize = mapsize;
        }

        public List<Entity> Generate()
        {
            var entities = new List<Entity>();

            float leftX = (mapsize / 4f);
            float rightX = (mapsize / 4f) * 3f;
            float upperY = (mapsize / 4f);
            float lowerY = (mapsize / 4f) * 3f;
            entities.Add(new Sun("Sun_10", 10f, new Vector(leftX, upperY)));
            entities.Add(new Sun("Sun_50", 50f, new Vector(rightX, upperY)));
            entities.Add(new Sun("Sun_75", 75f, new Vector(leftX, lowerY)));
            entities.Add(new Sun("Sun_100", 100f, new Vector(rightX, lowerY)));

            Vector spawnpoint = new Vector((mapsize / 4f) * 2f, (mapsize / 4f) * 3f);
            entities.Add(new Ship(new Guid(), "Bob", new Guid(), 100, 100, spawnpoint, Vector.FromAngleLength(45, 3)));

            return entities;
        }
    }
}
