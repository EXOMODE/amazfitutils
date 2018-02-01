using System;
using System.IO;
using System.Text;

namespace Resources.Models
{
    public class NewHeader : Header
    {
        public new const int HeaderSize = 0x24;
        public new const string ResSignature = "NERES";

        public NewHeader()
        {
            Signature = ResSignature;
        }

        public uint Unknown { get; set; }

        public override void WriteTo(BinaryWriter writer)
        {
            var buffer = new byte[HeaderSize];
            for (var i = 0; i < buffer.Length; i++) buffer[i] = 0xff;

            Encoding.ASCII.GetBytes(ResSignature).CopyTo(buffer, 0);
            buffer[5] = Version;
            BitConverter.GetBytes(Unknown).CopyTo(buffer, 0xa);
            BitConverter.GetBytes(ResourcesCount).CopyTo(buffer, 0x20);
            writer.Write(buffer);
        }

        public new static NewHeader ReadFrom(BinaryReader reader)
        {
            var buffer = reader.ReadBytes(HeaderSize);
            return new NewHeader
            {
                Signature = Encoding.ASCII.GetString(buffer, 0, 0x5),
                Version = buffer[0x5],
                Unknown = BitConverter.ToUInt32(buffer, 0xa),
                ResourcesCount = BitConverter.ToUInt32(buffer, 0x20)
            };
        }
    }
}