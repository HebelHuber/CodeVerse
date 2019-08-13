using System;
using System.Threading;

namespace CodeVerse.SimulatorTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var sim = new Simulator.Simulator();
            sim.GetNewMap();

            while(true)
            {
                sim.Simulate();
                Thread.Sleep(500);
            }
        }
    }
}
