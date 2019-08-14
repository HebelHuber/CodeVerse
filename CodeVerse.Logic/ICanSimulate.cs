using CodeVerse.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Logic
{
    public interface ICanSimulate
    {
        public List<Entity> GenerateMap();

        public List<Entity> Simulate(List<PlayerCommand> input);

        public List<Entity> Wipe();
    }
}
