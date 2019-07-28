using System;
using System.IO;
using System.Text;

namespace WatchFace.Parser.Models
{
    public class Header
    {
        private const string DialSignature = "HMDIAL\0";

        public string Signature { get; private set; } = DialSignature;
        public uint Unknown { get; set; }
        public uint ParametersSize { get; set; }
        public static int HeaderSize { get; set; } = 40;

        public bool IsValid => Signature == DialSignature;

        public void WriteTo(Stream stream)
        {
            var buffer = new byte[HeaderSize];

            for (var i = 0; i < buffer.Length; i++) buffer[i] = 0xff;

            Encoding.ASCII.GetBytes(Signature).CopyTo(buffer, 0);

            BitConverter.GetBytes(Unknown).CopyTo(buffer, HeaderSize == 40 ? 32 : 52);
            BitConverter.GetBytes(ParametersSize).CopyTo(buffer, HeaderSize == 40 ? 36 : 56);

            stream.Write(buffer, 0, HeaderSize);

            if (HeaderSize == 60) stream.Write(new byte[] { 0xff, 0xff, 0xff, 0xff }, 0, 4);
        }

        public static Header ReadFrom(Stream stream)
        {
            byte[] buffer = new byte[HeaderSize];

            stream.Read(buffer, 0, buffer.Length);
            string signature = Encoding.ASCII.GetString(buffer, 0, 7);

            uint unknown = BitConverter.ToUInt32(buffer, 32);
            uint parametersSize = BitConverter.ToUInt32(buffer, 36);

            if (unknown >= ushort.MaxValue || parametersSize >= ushort.MaxValue)
            {
                buffer = new byte[HeaderSize];
                stream.Position = 0;
                stream.Read(buffer, 0, buffer.Length);

                unknown = BitConverter.ToUInt32(buffer, 52);
                parametersSize = BitConverter.ToUInt32(buffer, 56);

                stream.Position += 4;
            }

            return new Header
            {
                Signature = signature,
                Unknown = unknown,
                ParametersSize = parametersSize,
            };
        }
    }
}