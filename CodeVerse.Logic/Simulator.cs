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
        protected float GravityMultiplier = 1f;


        public List<Entity> GetDebugEntities()
        {
            if (debugMode)
                return entities;
            else
                throw new Exception("Simulator not in debug mode, no access to entities.");
        }

        protected void Init(int seed, float mapsize, float GravityMultiplier, bool debugmode)
        {
            this.debugMode = debugmode;
            this.mapsize = mapsize;
            this.GravityMultiplier = GravityMultiplier;

            if (seed == 0)
                StaticRandom.SetRandomSeed();
            else
                StaticRandom.SetSeed(seed);

            this.seed = seed;

            entities = new List<Entity>();

            MapGen();
        }

        public abstract void MapGen();

        public List<ScannerContent> Simulate(List<PlayerCommand> input = null)
        {
            if (debugMode)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var result = SimulateInternal(input);
                stopwatch.Stop();
                Console.WriteLine("Simulation Time: {0}ms", stopwatch.ElapsedMilliseconds);
                return result;
            }
            else
                return SimulateInternal(input);
        }

        protected abstract List<ScannerContent> SimulateInternal(List<PlayerCommand> input = null);
    }
}
