namespace CodeVerse.Common
{
    public class Sun : StaticEntity
    {
        public static Sun Random(string name, float maxMapSize)
        {
            var newObj = new Sun();
            newObj.name = name;
            newObj.radius = StaticRandom.randomFloatInRange(50, 80);
            newObj.pos = StaticRandom.RandomVecInSquare(newObj.radius, maxMapSize - newObj.radius);
            newObj.Gravity = StaticRandom.randomNormalizedFloat * 1f;

            return newObj;
        }
    }
}
