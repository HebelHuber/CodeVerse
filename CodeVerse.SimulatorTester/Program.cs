using System;

namespace CodeVerse.SimulatorTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var sim = new Simulator.Simulator();
            sim.GetNewMap();
            sim.Simulate();
        }
    }
}
