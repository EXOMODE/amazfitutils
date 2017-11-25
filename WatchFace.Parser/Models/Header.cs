using System;
using System.IO;
using System.Text;

namespace WatchFace.Parser.Models
{
    public class Header
    {
        public const int HeaderSize = 40;
        private const string DialSignature = "HMDIAL\0";

        public string Signature { get; private set; } = DialSignature;
        public uint Unknown { get; set; }
        public uint ParametersSize { get; set; }

        public bool IsValid => Signature == DialSignature;

        public void WriteTo(Stream stream)
        {
            var buffer = new byte[HeaderSize];
            for (var i = 0; i < buffer.Length; i++) buffer[i] = 0xff;

            var signature = Encoding.ASCII.GetBytes(Signature);
            var unknown = BitConverter.GetBytes(Unknown);
            var parametersSize = BitConverter.GetBytes(ParametersSize);

            signature.CopyTo(buffer, 0);
            unknown.CopyTo(buffer, 32);
            parametersSize.CopyTo(buffer, 36);
            stream.Write(buffer, 0, HeaderSize);
        }

        public static Header ReadFrom(Stream stream)
        {
            var buffer = new byte[HeaderSize];
            stream.Read(buffer, 0, HeaderSize);

            return new Header
            {
                Signature = Encoding.ASCII.GetString(buffer, 0, 7),
                Unknown = BitConverter.ToUInt32(buffer, 32),
                ParametersSize = BitConverter.ToUInt32(buffer, 36)
            };
        }
    }
}