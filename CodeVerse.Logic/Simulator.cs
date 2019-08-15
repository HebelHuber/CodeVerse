using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CodeVerse.Common;


namespace CodeVerse.Logic
{
    public abstract class Simulator
    {
        public bool IsDebugMode() { return debugMode; }

        protected int seed;
        protected List<Entity> entities;
        public float mapsize;
        protected bool debugMode;

        public List<Entity> GetDebugEntities()
        {
            if (debugMode)
                return entities;
            else
                throw new Exception("Simulator not in debug mode, no access to entities.");
        }

        protected Simulator(int seed = 0, float mapsize = 1000f, bool debugmode = false)
        {

            if (seed == 0)
                StaticRandom.SetRandomSeed();
            else
                StaticRandom.SetSeed(seed);

            this.debugMode = debugmode;
            this.mapsize = mapsize;
            this.seed = seed;
        }

        public abstract void GenerateMap();

        public List<ScannerContent> Simulate(List<PlayerCommand> input = null)
        {
            if (debugMode)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var result = SimulateInternal(input);
                stopwatch.Stop();

                if (stopwatch.ElapsedMilliseconds > 50)
                    Console.WriteLine("Simulation Time: {0}ms", stopwatch.ElapsedMilliseconds);

                return result;
            }
            else
                return SimulateInternal(input);
        }

        protected abstract List<ScannerContent> SimulateInternal(List<PlayerCommand> input = null);
    }
}
