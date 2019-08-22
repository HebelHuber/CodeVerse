using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace CodeVerse.Server.Networking
{
    public class Player
    {
        TcpClient client;
        NetworkStream stream;

        public Player(TcpClient client)
        {
            this.client = client;
            stream = client.GetStream();
        }

        public void SendPacket(Packet packet)
        {
            stream.Write(packet.Serialize());
        }

        public Packet[] ReceivePackets()
        {
            List<Packet> _packets = new List<Packet>();
            if (stream.DataAvailable)
            {
                byte[] _buffer = new byte[client.ReceiveBufferSize];
                int _bytesRead = stream.Read(_buffer, 0, client.ReceiveBufferSize);
                using (MemoryStream _ms = new MemoryStream(_buffer))
                {
                    using (BinaryReader _reader = new BinaryReader(_ms))
                    {
                        while (_reader.BaseStream.Position != _bytesRead)
                        {
                            _packets.Add(new Packet(_reader.ReadBytes(_reader.ReadUInt16())));
                        }
                    }
                }
            }
            return _packets.ToArray();
        }

        ~Player()
        {
            client.Dispose();
        }
    }
}
