namespace CodeVerse.Common
{
    public class Moon : StaticEntity
    {
        public static Moon Random(string name)
        {
            var newObj = new Moon();
            newObj.pos = new Vector(StaticExtensions.randomNormalizedFloat * 50f, StaticExtensions.randomNormalizedFloat * 50f);
            newObj.name = name;
            newObj.radius = StaticExtensions.randomNormalizedFloat * 20f;
            newObj.Gravity = 0f;
            return newObj;
        }
    }
}
