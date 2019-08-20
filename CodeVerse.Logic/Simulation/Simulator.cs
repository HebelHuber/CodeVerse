using CodeVerse.Common;
using CodeVerse.Common.Commands;
using CodeVerse.Common.data;
using CodeVerse.Logic.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Logic.Simulation
{
    public abstract class Simulator
    {
        public List<Entity> entities;

        public void SetMap(List<Entity> map)
        {
            entities = map;
        }

        public abstract List<shipData> Simulate(List<PlayerCommand> input = null);
    }
}
