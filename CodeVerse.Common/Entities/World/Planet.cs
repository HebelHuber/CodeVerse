namespace CodeVerse.Common
{
    public class Planet : StaticEntity
    {
        public static Planet Random(string name)
        {
            var newObj = new Planet();
            newObj.pos = new Vector(StaticRandom.randomNormalizedFloat * 50f, StaticRandom.randomNormalizedFloat * 50f);
            newObj.name = name;
            newObj.radius = StaticRandom.randomNormalizedFloat * 20f;
            newObj.Gravity = StaticRandom.randomNormalizedFloat * 1f;
            return newObj;
        }
    }
}
