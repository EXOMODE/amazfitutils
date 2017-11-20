using System;
using System.IO;
using System.Text;

namespace WatchFace.Models
{
    public class Header
    {
        public const int HeaderSize = 40;
        private const string DialSignature = "HMDIAL\0";
        private readonly byte[] _header;

        public Header(byte[] header)
        {
            _header = header;
        }

        public string Signature => Encoding.ASCII.GetString(_header, 0, 7);
        public bool IsValid => Signature == DialSignature;
        public uint Unknown => BitConverter.ToUInt32(_header, 32);
        public uint ParametersSize => BitConverter.ToUInt32(_header, 36);

        public static Header ReadFrom(Stream fileStream)
        {
            var buffer = new byte[HeaderSize];
            fileStream.Read(buffer, 0, HeaderSize);
            return new Header(buffer);
        }
    }
}