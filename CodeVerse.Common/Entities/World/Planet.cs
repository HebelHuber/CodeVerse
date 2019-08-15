namespace CodeVerse.Common
{
    public class Planet : StaticEntity
    {
        public static Planet Random(string name, float maxMapSize)
        {
            var newObj = new Planet();
            newObj.name = name;
            newObj.radius = StaticRandom.randomFloatInRange(15, 25);
            newObj.mass = newObj.radius;
            newObj.pos = StaticRandom.RandomVecInSquare(newObj.radius, maxMapSize - newObj.radius);
            return newObj;
        }
    }
}
