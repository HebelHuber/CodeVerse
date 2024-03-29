﻿using System;
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
