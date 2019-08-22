using System.IO;

namespace CodeVerse.Server.Networking
{
    public class Packet
    {
        public byte[] Data { get; private set; }

        ushort dataLength;

        public Packet(byte[] data)
        {
            dataLength = (ushort)data.Length;
            Data = data;
        }

        public byte[] Serialize()
        {
            using (MemoryStream _ms = new MemoryStream())
            {
                using (BinaryWriter _writer = new BinaryWriter(_ms))
                {
                    _writer.Write(dataLength);
                    _writer.Write(Data);
                    return ((MemoryStream)_writer.BaseStream).ToArray();
                }
            }
        }
    }
}
