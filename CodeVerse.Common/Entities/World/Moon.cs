namespace CodeVerse.Common
{
    public class Moon : StaticEntity
    {
        public static Moon Random(string name, float maxMapSize)
        {
            var newObj = new Moon();
            newObj.name = name;
            newObj.radius = StaticRandom.randomFloatInRange(3, 15);
            newObj.mass = newObj.radius;
            newObj.pos = StaticRandom.RandomVecInSquare(newObj.radius, maxMapSize - newObj.radius);
            return newObj;
        }
    }
}
