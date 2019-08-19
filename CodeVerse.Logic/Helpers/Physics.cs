using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeVerse.Common
{
    public static class Physics
    {
        public static bool CollidesWith(this Entity a, Entity b)
        {
            return CircleCollision(a.pos, a.radius, b.pos, b.radius);
        }

        public static List<Entity> CollidesWithMultiple(this Entity e, List<Entity> others)
        {
            var collisions = new List<Entity>();

            foreach (var item in others.Where(q => q != e))
            {
                if (e.CollidesWith(item))
                {
                    collisions.Add(item);
                }
            }

            return collisions;
        }

        public static bool isInsideEntity(this Vector pos, Entity e)
        {
            return Vector.VecFromTo(pos, e.pos).Length < e.radius;
        }

        public static bool isOutsideEntity(this Vector pos, Entity e)
        {
            return !pos.isInsideEntity(e);
        }

        public static bool includesPosition(this Entity e, Vector pos)
        {
            return pos.isInsideEntity(e);
        }

        private static bool CircleCollision(Vector v1, float r1, Vector v2, float r2)
        {
            float distSq = (v1.X - v2.X) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v1.Y - v2.Y);
            float radSumSq = (r1 + r2) * (r1 + r2);

            if (distSq == radSumSq) // touching
                return true;
            else if (distSq > radSumSq) // no touchy touchy
                return false;
            else // intersection
                return true;
        }
    }
}
