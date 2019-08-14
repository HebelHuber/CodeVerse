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
                if (item is Sun)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else if (item is Planet)
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (item is Moon)
                    Console.ForegroundColor = ConsoleColor.Gray;
                else if (item is Ship)
                    Console.ForegroundColor = ConsoleColor.Cyan;
                else if (item is Bullet)
                    Console.ForegroundColor = ConsoleColor.Magenta;

                Console.WriteLine(item.ToStringWithProps());
                Console.ResetColor();
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
