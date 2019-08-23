﻿using CodeVerse.Common;
using CodeVerse.Common.Commands;
using CodeVerse.Common.data;
using CodeVerse.Common.Networking;
using CodeVerse.Logic.Maps;
using CodeVerse.Logic.Simulation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CodeVerse.Logic
{
    public class Universe
    {
        public Simulator _sim;
        public Guid _guid;
        ConcurrentDictionary<Guid, Player> Players = new ConcurrentDictionary<Guid, Player>();

        public List<Entity> entities
        {
            get
            {
                // return a copy so it can't be modified outside,
                // also for thread safetiy when drawing
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

        public void AddPlayer(Player player)
        {
            // Only add player he is not allready in universe
            if (!Players.ContainsKey(player.Guid))
            {
                // TODO: Send some notification that a new player joined
                Players[player.Guid] = player;
            }
        }

        public List<shipData> Simulate(List<PlayerCommand> input = null)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            _sim.Simulate(input.Where(q => !(q is ScanCommand)).ToList());
            var result = _sim.Scan(input.Where(q => q is ScanCommand).ToList());

            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds > 50)
                Console.WriteLine("Simulation Time: {0}ms", stopwatch.ElapsedMilliseconds);

            return result;
        }
    }
}
