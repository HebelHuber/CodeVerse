using System.IO;

namespace CodeVerse.Common.Networking
{
    public class Packet
    {
        public byte[] Data { get; private set; }

        public ushort DataLength { get; private set; }

        public Packet(byte[] data)
        {
            DataLength = (ushort)data.Length;
            Data = data;
        }
        public byte[] Serialize()
        {
            using (MemoryStream _ms = new MemoryStream())
            {
                using (BinaryWriter _writer = new BinaryWriter(_ms))
                {
                    _writer.Write(DataLength);
                    _writer.Write(Data);
                    return ((MemoryStream)_writer.BaseStream).ToArray();
                }
            }
        }
    }
}
