﻿using CodeVerse.Common;
using CodeVerse.Logic;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CodeVerse.LogicTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var sim = new DefaultSimulator();

            //PrintEntities(sim.GenerateMap());
            PrintEntities(sim.GenerateMap(123));

            while (true)
            {
                Console.Clear();
                PrintEntities(sim.Simulate(new List<PlayerCommand>()), true);
                Thread.Sleep(150);
            }

            Console.ReadKey(true);
        }

        private static void PrintEntities(List<Entity> ents, bool dynamicOnly = false)
        {
            foreach (var item in ents)
            {
                if (dynamicOnly && !(item is MovingEntity))
                    continue;

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
        }
    }
}
