namespace CodeVerse.Common
{
    public class Moon : StaticEntity
    {
        public static Moon Random(string name, float maxMapSize)
        {
            var newObj = new Moon();
            newObj.name = name;
            newObj.radius = StaticRandom.randomFloatInRange(3, 8);
            newObj.pos = StaticRandom.RandomVecInSquare(newObj.radius, maxMapSize - newObj.radius);
            newObj.Gravity = 0f;

            return newObj;
        }
    }
}
