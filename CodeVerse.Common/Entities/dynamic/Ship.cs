namespace CodeVerse.Common
{
    public class Ship : MovingEntity
    {
        public string Owner;
        public float HP;
        public float Energy;

        public static Ship Random(string owner, string name, float maxMapSize)
        {
            var newObj = new Ship();
            newObj.name = name;
            newObj.Owner = owner;
            newObj.HP = 100;
            newObj.Energy = 100;
            newObj.Weight = 500f;
            newObj.radius = 5f;
            newObj.pos = StaticRandom.RandomVecInSquare(newObj.radius, maxMapSize - newObj.radius);
            newObj.Velocity = new Vector(0, 0);
            return newObj;
        }
    }
}
