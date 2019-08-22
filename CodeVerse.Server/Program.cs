using CodeVerse.Server.Networking;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace CodeVerse.Server
{
    class Program
    {
        // This is not thread safe when modifying a Player object
        static ConcurrentBag<Player> players = new ConcurrentBag<Player>();

        static void Main(string[] args)
        {
            Console.WriteLine("Server starting...");

            // Initialize stuff here!
            // e.g. create universes (each universe in its own thread?)

            // Start accepting connections
            Networking.Server.PlayerConnected += OnPlayerConnected;
            Networking.Server.Start(45385);

            // Debug client to test
            var client = new System.Net.Sockets.TcpClient("localhost", 45385);

            //Do some looping here? Keep server alive
            while (true)
            {
                // Receive packets since last tick
                foreach (var player in players)
                {
                    foreach (var pck in player.ReceivePackets())
                    {
                        Console.WriteLine(pck.Data[3]);
                        // Send something back to debug client
                        player.SendPacket(new Packet(new byte[] { 0, 1, 2, 3 }));
                    }
                }
                Thread.Sleep(100);
                // Send something from debug client to server
                var packet = new Packet(new byte[] { 0, 1, 2, 3 });
                client.GetStream().Write(packet.Serialize());
                packet = new Packet(new byte[] { 3, 2, 1, 0 });
                client.GetStream().Write(packet.Serialize());
            }
        }

        static void OnPlayerConnected(Player player)
        {
            players.Add(player);
        }
    }
}
