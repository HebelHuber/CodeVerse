using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CodeVerse.Common;
using CodeVerse.Common.Commands;
using CodeVerse.Common.data;
using CodeVerse.Logic.Maps;
using CodeVerse.Logic.Simulation;

namespace CodeVerse.Logic
{
    public class Universe
    {
        public Simulator _sim;
        public Guid _guid;

        public List<Entity> entities
        {
            get
            {
                return new List<Entity>(_sim.entities);
            }
        }

        public Universe(Guid? guid = null, Simulator sim = null, IMapGenerator mapgen = null)
        {
            if (guid == null)
                _guid = new Guid();
            else
                _guid = (Guid)guid;

            if (mapgen == null)
                mapgen = new RandomMap();

            if (sim == null)
                _sim = new DefaultSimulator();
            else
                _sim = sim;

            _sim.SetMap(mapgen.Generate());
        }

        public List<shipData> Simulate(List<PlayerCommand> input = null)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _sim.Simulate(input);
            var result = _sim.Scan(input);
            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds > 50)
                Console.WriteLine("Simulation Time: {0}ms", stopwatch.ElapsedMilliseconds);

            return result;
        }
    }
}
