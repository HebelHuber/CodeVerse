namespace CodeVerse.Common
{
    public class Planet : StaticEntity
    {
        public static Planet Random(string name)
        {
            var newObj = new Planet();
            newObj.pos = new Vector(StaticExtensions.randomNormalizedFloat * 50f, StaticExtensions.randomNormalizedFloat * 50f);
            newObj.name = name;
            newObj.radius = StaticExtensions.randomNormalizedFloat * 20f;
            newObj.Gravity = StaticExtensions.randomNormalizedFloat * 1f;
            return newObj;
        }
    }
}
