namespace CodeVerse.Common
{
    public class Planet : StaticEntity
    {
        public static Planet Random(string name, float maxMapSize)
        {
            var newObj = new Planet();
            newObj.name = name;
            newObj.radius = StaticRandom.randomFloatInRange(15, 25);
            newObj.pos = StaticRandom.RandomVecInSquare(newObj.radius, maxMapSize - newObj.radius);
            //newObj.Gravity = StaticRandom.randomNormalizedFloat * 0.1f;
            newObj.Gravity = 0f;

            return newObj;
        }
    }
}
