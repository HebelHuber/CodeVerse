using CodeVerse.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Simulator
{
    public interface ICanSimulate
    {
        public List<Entity> GenerateMap();

        public List<Entity> Simulate(List<Entity> input);

        public List<Entity> Wipe();
    }
}
