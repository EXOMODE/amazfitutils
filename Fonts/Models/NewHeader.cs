using System;
using System.IO;
using System.Text;

namespace Fonts.Models
{
    public class NewHeader
    {
        public const int HeaderSize = 0x22;
        public const string ResSignature = "NEZK";

        public string Signature { get; protected set; } = ResSignature;
        public byte Version { get; set; }
        public ushort BlocksCount { get; set; }

        public uint Unknown { get; set; }

        public void WriteTo(BinaryWriter writer)
        {
            var buffer = new byte[HeaderSize];
            for (var i = 0; i < buffer.Length; i++) buffer[i] = 0xff;

            Encoding.ASCII.GetBytes(ResSignature).CopyTo(buffer, 0);
            buffer[0x4] = Version;
            BitConverter.GetBytes(Unknown).CopyTo(buffer, 0xa);
            BitConverter.GetBytes(BlocksCount).CopyTo(buffer, 0x20);
            writer.Write(buffer);
        }

        public static NewHeader ReadFrom(BinaryReader reader)
        {
            var buffer = reader.ReadBytes(HeaderSize);
            return new NewHeader
            {
                Signature = Encoding.ASCII.GetString(buffer, 0, 0x4),
                Version = buffer[0x4],
                Unknown = BitConverter.ToUInt32(buffer, 0xa),
                BlocksCount = BitConverter.ToUInt16(buffer, 0x20)
            };
        }
    }
}
