namespace CodeVerse.Common
{
    public class Sun : StaticEntity
    {
        public Sun(string name, float radius, Vector pos, float mass = 0)
        {
            this.name = name;
            this.pos = pos;
            this.radius = radius;

            if (mass == 0)
                this.mass = this.radius;
            else
                this.mass = mass;
        }

        public Sun() { }

        public static Sun Random(string name, float maxMapSize)
        {
            var newObj = new Sun();
            newObj.name = name;
            newObj.radius = StaticRandom.randomNormalizedFloat.Remap(0, 1, 50, 100);
            newObj.mass = newObj.radius;
            newObj.pos = StaticRandom.RandomVecInSquare(newObj.radius, maxMapSize - newObj.radius);

            return newObj;
        }
    }
}
