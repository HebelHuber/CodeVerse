namespace CodeVerse.Common
{
    public class Moon : StaticEntity
    {
        public static Moon Random(string name)
        {
            var newObj = new Moon();
            newObj.pos = new Vector(StaticRandom.randomNormalizedFloat * 50f, StaticRandom.randomNormalizedFloat * 50f);
            newObj.name = name;
            newObj.radius = StaticRandom.randomNormalizedFloat * 20f;
            newObj.Gravity = 0f;
            return newObj;
        }
    }
}
