using CodeVerse.Common;
using CodeVerse.Simulator;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CodeVerse.SimulatorTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var sim = new DefaultSimulator();
            var ents = sim.GenerateMap();

            foreach (var item in ents)
            {
                Console.WriteLine(item.ToStringWithProps());
            }

            //while (true)
            //{
            //var output = sim.Simulate(new List<Entity>());
            //Thread.Sleep(500);
            //}

            Thread.Sleep(-1);
        }
    }
}
