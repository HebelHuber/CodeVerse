using CodeVerse.Common.Networking;
using CodeVerse.Logic;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace CodeVerse.Server
{
    public static class Networking
    {
        public static event Action<Player> PlayerConnected;

        static List<Universe> Universes { get; } = new List<Universe>();

        static TcpListener listener;

        public static void Start(int port)
        {
            Listen(port);
        }

        public static void Stop()
        {
            listener.Stop();
        }

        static void Listen(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine(" >> Server Started. Listening on port " + port);
            listener.BeginAcceptTcpClient(AcceptClient, null);
        }

        static void AcceptClient(IAsyncResult ar)
        {
            var _client = listener.EndAcceptTcpClient(ar);
            PlayerConnected?.Invoke(new Player(_client));
            Console.WriteLine($" >> {_client.Client.RemoteEndPoint} connected!");
            listener.BeginAcceptTcpClient(AcceptClient, null);
        }
    }
}
