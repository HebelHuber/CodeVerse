using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Common
{
    public class Bullet : MovingEntity
    {
        public string origin;

        public Bullet() { }

        public Bullet(string name, string origin, Vector pos, Vector vel, float mass = 0f)
        {
            this.name = name;
            this.origin = origin;
            this.pos = pos;
            this.Velocity = vel;
            this.radius = 2.5f;

            if (mass == 0)
                this.mass = this.radius;
            else
                this.mass = mass;
        }

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
