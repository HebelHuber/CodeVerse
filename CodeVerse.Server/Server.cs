using CodeVerse.Common.Networking;
using CodeVerse.Logic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace CodeVerse.Server
{
    class Server
    {
        static List<Universe> Universes { get; } = new List<Universe>();
        // This is not thread safe when modifying a Player object
        static ConcurrentBag<Player> players = new ConcurrentBag<Player>();

        static void Main(string[] args)
        {
            Console.WriteLine("Server starting...");

            // Initialize stuff here!
            // e.g. create universes (each universe in its own thread?)

            // Start accepting connections
            Networking.PlayerConnected += OnPlayerConnected;
            Networking.Start(45385);

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
                        Console.WriteLine("Client -> Server: " + pck.Data[3]);
                        // Send something back to debug client
                        player.SendPacket(new Packet(pck.Data));
                    }
                }
                Console.WriteLine("Tick");
                Thread.Sleep(100);
                // Send something from debug client to server
                var packet = new Packet(new byte[] { 0, 1, 2, 3 });
                client.GetStream().Write(packet.Serialize());
                packet = new Packet(new byte[] { 3, 2, 1, 0 });
                client.GetStream().Write(packet.Serialize());
                while (client.GetStream().DataAvailable)
                {
                    System.IO.BinaryReader _reader = new System.IO.BinaryReader(client.GetStream());
                    Packet _packet = new Packet(_reader.ReadBytes(_reader.ReadUInt16()));
                    Console.WriteLine("Server -> Client: " + _packet.Data[3]);
                }
            }
        }

        static void OnPlayerConnected(Player player)
        {
            players.Add(player);
        }
    }
}
