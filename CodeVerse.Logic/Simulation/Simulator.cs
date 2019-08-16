﻿using CodeVerse.Common;
using CodeVerse.Common.Commands;
using CodeVerse.Common.data;
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

        public abstract void Simulate(List<PlayerCommand> input = null);
        public abstract List<shipData> Scan(List<PlayerCommand> input = null);
    }
}