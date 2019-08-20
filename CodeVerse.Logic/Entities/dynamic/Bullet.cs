using System;
using System.Collections.Generic;
using System.Text;
using CodeVerse.Common;

namespace CodeVerse.Logic.Entities
{
    public class Bullet : MovingEntity
    {
        public Guid originID;

        public Bullet() { }

        public Bullet(string name, Guid originID, Vector pos, Vector vel, float mass = 0f)
        {
            this.name = name;
            this.originID = originID;
            this.pos = pos;
            this.Velocity = vel;
            this.radius = 2.5f;

            if (mass == 0)
                this.mass = this.radius;
            else
                this.mass = mass;
        }

        public static Bullet Random(Guid originID, float mapsize)
        {
            var bullet = new Bullet();
            bullet.name = "Bullet_0";
            bullet.originID = originID;
            bullet.pos = StaticRandom.RandomVecInSquare(0, mapsize);
            bullet.Velocity = new Vector(500, 100) * 0;
            bullet.radius = 2.5f;
            bullet.mass = bullet.radius;
            return bullet;
        }
    }
}
