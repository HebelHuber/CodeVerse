using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace CodeVerse.Common.Networking
{
    public class Player
    {
        public Guid Guid { get; }

        TcpClient client;
        BinaryReader reader;
        BinaryWriter writer;
        BufferedStream writeBuffer;

        public Player(TcpClient client)
        {
            this.client = client;
            this.client.NoDelay = true;
            writeBuffer = new BufferedStream(client.GetStream());
            reader = new BinaryReader(client.GetStream());
            writer = new BinaryWriter(writeBuffer);
        }

        public void SendPacket(Packet packet)
        {
            writer.Write(packet.DataLength);
            writer.Write(packet.Data);
            writeBuffer.Flush();
        }

        public Packet[] ReceivePackets()
        {
            List<Packet> _packets = new List<Packet>();
            while (((NetworkStream)reader.BaseStream).DataAvailable)
            {
                _packets.Add(new Packet(reader.ReadBytes(reader.ReadUInt16())));
            }
            return _packets.ToArray();
        }

        ~Player()
        {
            reader.Dispose();
            writer.Dispose();
            client.Dispose();
        }
    }
}
