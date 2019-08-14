using System;
using CodeVerse.Common;
using CodeVerse.Logic;

namespace CodeVerse.Networking
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var sim = new DefaultSimulator();
            sim.GenerateMap();
        }
    }
}
