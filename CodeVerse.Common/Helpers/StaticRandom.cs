using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Common
{
    public static class StaticRandom
    {
        private static Random random = null;

        public static void SetRandomSeed()
        {
            random = new Random();
        }

        public static void SetSeed(int seed)
        {
            random = new Random(seed);
        }

        public static float randomNormalizedFloat
        {
            get
            {
                if (random == null)
                    throw new Exception("Random not initialized.\n" +
                    "Use StaticRandom.SetSeed(int seed)\n" +
                    "or StaticRandom.SetRandomSeed()\n" +
                    "to set a seed.");

                return Convert.ToSingle(random.NextDouble());
            }
        }

        public static float randomFloatInRange(float min = 0, float max = 1)
        {
            if (random == null)
                throw new Exception("Random not initialized.\n" +
                "Use StaticRandom.SetSeed(int seed)\n" +
                "or StaticRandom.SetRandomSeed()\n" +
                "to set a seed.");

            if (min == max)
                return min;

            return randomNormalizedFloat.Remap(0, 1, min, max, true);
        }

        public static Vector RandomVecInCircle(float min = 0, float max = 1)
        {
            float xVal = randomNormalizedFloat;
            return new Vector(xVal, 1 - xVal) * randomFloatInRange(min, max);
        }

        public static Vector RandomVecInSquare(float min = 0, float max = 1)
        {
            float xVal = randomFloatInRange(min, max);
            float yVal = randomFloatInRange(min, max);
            return new Vector(xVal, yVal);
        }


        public static int randomInt(int min, int max)
        {
            if (random == null)
                throw new Exception("Random not initialized.\n" +
                "Use StaticRandom.SetSeed(int seed)\n" +
                "or StaticRandom.SetRandomSeed()\n" +
                "to set a seed.");

            if (min == max)
                return min;

            int rnd = random.Next(max - min + 1);
            return rnd + min;
        }
    }
}
