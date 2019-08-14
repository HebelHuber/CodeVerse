namespace CodeVerse.Common
{
    public class Sun : StaticEntity
    {
        public static Sun Random(string name)
        {
            var newObj = new Sun();
            newObj.pos = new Vector(StaticRandom.randomNormalizedFloat * 50f, StaticRandom.randomNormalizedFloat * 50f);
            newObj.name = name;
            newObj.radius = StaticRandom.randomNormalizedFloat * 20f;
            newObj.Gravity = StaticRandom.randomNormalizedFloat * 10f;
            return newObj;
        }
    }
}
