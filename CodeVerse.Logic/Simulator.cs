using System;
using System.Collections.Generic;
using System.Text;
using CodeVerse.Common;


namespace CodeVerse.Logic
{
    public abstract class Simulator
    {
        public bool IsDebugMode() { return EntitiesArePublic; }

        protected int seed;
        protected List<Entity> entities;
        public float mapsize;
        private bool EntitiesArePublic;
        protected float GravityMultiplier = 1f;


        public List<Entity> GetDebugEntities()
        {
            if (EntitiesArePublic)
                return entities;
            else
                throw new Exception("Simulator not in debug mode, no access to entities.");
        }

        protected void Init(int seed, float mapsize, float GravityMultiplier, bool debugmode)
        {
            this.EntitiesArePublic = debugmode;
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

        public abstract List<ScannerContent> Simulate(List<PlayerCommand> input = null);
    }
}
