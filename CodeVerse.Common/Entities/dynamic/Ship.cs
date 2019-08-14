namespace CodeVerse.Common
{
    public class Ship : MovingEntity
    {
        public string Owner;
        public float HP;
        public float Energy;

        public static Ship Random(string owner, string name)
        {
            var ship = new Ship();
            ship.Velocity = new Vector();
            ship.name = name;
            ship.radius = 5f;
            ship.Owner = owner;
            ship.HP = 100;
            ship.Energy = 100;
            ship.Weight = 500f;
            ship.pos = new Vector(StaticRandom.randomNormalizedFloat * 50f, StaticRandom.randomNormalizedFloat * 50f);
            return ship;
        }
    }
}
