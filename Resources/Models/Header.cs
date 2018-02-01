using System;
using System.IO;
using System.Text;

namespace Resources.Models
{
    public class Header
    {
        public const int HeaderSize = 20;
        public const string ResSignature = "HMRES";

        public string Signature { get; protected set; } = ResSignature;
        public byte Version { get; set; }
        public uint ResourcesCount { get; set; }

        public virtual void WriteTo(BinaryWriter writer)
        {
            var buffer = new byte[HeaderSize];
            for (var i = 0; i < buffer.Length; i++) buffer[i] = 0xff;

            Encoding.ASCII.GetBytes(ResSignature).CopyTo(buffer, 0);
            buffer[5] = Version;
            BitConverter.GetBytes(ResourcesCount).CopyTo(buffer, 16);
            writer.Write(buffer);
        }

        public static Header ReadFrom(BinaryReader reader)
        {
            var buffer = reader.ReadBytes(HeaderSize);
            return new Header
            {
                Signature = Encoding.ASCII.GetString(buffer, 0, 5),
                Version = buffer[5],
                ResourcesCount = BitConverter.ToUInt32(buffer, 16)
            };
        }
    }
}