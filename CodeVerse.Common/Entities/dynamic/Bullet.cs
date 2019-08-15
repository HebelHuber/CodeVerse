using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Common
{
    public class Bullet : MovingEntity
    {
        public string origin;

        public static Bullet Random(string origin, float mapsize)
        {
            var bullet = new Bullet();
            bullet.name = "Bullet_0";
            bullet.origin = origin;
            bullet.pos = StaticRandom.RandomVecInSquare(0, mapsize);
            bullet.Velocity = new Vector(500, 100) * 0;
            bullet.radius = 2.5f;
            bullet.mass = bullet.radius;
            return bullet;
        }
    }
}
