namespace CodeVerse.Common
{
    public class Sun : StaticEntity
    {
        public static Sun Random(string name)
        {
            var newObj = new Sun();
            newObj.pos = new Vector(StaticExtensions.randomNormalizedFloat * 50f, StaticExtensions.randomNormalizedFloat * 50f);
            newObj.name = name;
            newObj.radius = StaticExtensions.randomNormalizedFloat * 20f;
            newObj.Gravity = StaticExtensions.randomNormalizedFloat * 10f;
            return newObj;
        }
    }
}
