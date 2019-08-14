using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Common
{
    public class Bullet : MovingEntity
    {
        public string origin;

        public static Bullet Random(string origin)
        {
            var bullet = new Bullet();
            bullet.name = "Bullet_0";
            bullet.origin = origin;
            bullet.pos = new Vector();
            bullet.Velocity = new Vector(500, 100);
            bullet.radius = 1f;
            bullet.Weight = 50f;
            return bullet;
        }
    }
}
