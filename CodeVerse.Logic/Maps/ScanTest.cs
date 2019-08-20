using CodeVerse.Common;
using CodeVerse.Logic.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Logic.Maps
{
    public class ScanTest : IMapGenerator
    {
        private float mapsize;

        public ScanTest(float mapsize = 1000f)
        {
            this.mapsize = mapsize;
        }

        public List<Entity> Generate()
        {
            var entities = new List<Entity>();

            var scanner = new Ship(new Guid(), "bob", new Guid(), 100, 100, new Vector(1, 1) * (mapsize / 2), Vector.Zero);
            entities.Add(scanner);

            var scan = new Scan();
            scan.pos = scanner.pos;
            scan.range = 100f;
            scan.leftEdgeAngle = 0;
            scan.rightEdgeAngle = 100f;
            scan.name = "SCAN";
            entities.Add(scan);

            int count = 8;
            float piepiecesize = 360f / count;

            for (int i = 0; i < count; i++)
            {
                float angle = i * piepiecesize;

                var sunPos = scanner.pos + Vector.FromAngleLength(angle, 350);
                var tempE = new Sun("sun" + i, 50, sunPos, mass: 0);

                entities.Add(tempE);

                var scanHit = tempE.isInsideScanArc(scanner, 0, 90, 100, true);
                Console.WriteLine(tempE.name + ": pos: " + tempE.pos + " angle: " + angle +
                    "\n" + scanner.name + ": pos: " + scanner.pos +
                    "\n Distance: " + Physics.Distance(tempE, scanner) +
                    "\n Scanhit: " + scanHit);
                Console.WriteLine("");
            }

            return entities;
        }
    }
}
