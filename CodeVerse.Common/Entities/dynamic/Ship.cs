﻿namespace CodeVerse.Common
{
    public class Ship : MovingEntity
    {
        public string Owner;
        public float HP;
        public float Energy;

        public Ship(long ID, string name, string owner, float hP, float energy, Vector pos, Vector velocity, float mass = 0f, float radius = 5f)
        {
            this.ID = ID;
            this.name = name;
            this.pos = pos;

            this.radius = radius;

            if (mass == 0)
                this.mass = this.radius;
            else
                this.mass = mass;

            this.Velocity = velocity;
            this.Owner = owner;
            this.HP = hP;
            this.Energy = energy;
        }

        public Ship() { }

        public static Ship Random(long ID, string owner, string name, float maxMapSize)
        {
            var newObj = new Ship();
            newObj.ID = ID;
            newObj.name = name;
            newObj.Owner = owner;
            newObj.HP = 100;
            newObj.Energy = 100;
            newObj.radius = 5f;
            newObj.mass = newObj.radius;
            newObj.pos = StaticRandom.RandomVecInSquare(newObj.radius, maxMapSize - newObj.radius);
            newObj.Velocity = new Vector(0, 0);
            return newObj;
        }
    }
}
