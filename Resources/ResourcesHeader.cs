using System;
using System.IO;
using System.Text;

namespace Resources
{
    public class ResourcesHeader
    {
        public const int HeaderSize = 20;
        private const string ResHeaderSignature = "HMRES";
        private readonly byte[] _header;

        public ResourcesHeader(byte[] header)
        {
            _header = header;
        }

        public string Signature => Encoding.ASCII.GetString(_header, 0, 5);
        public bool IsValid => Signature == ResHeaderSignature;
        public uint Version => _header[5];
        public uint ResourcesCount => BitConverter.ToUInt32(_header, 16);

        public static ResourcesHeader ReadFrom(BinaryReader reader)
        {
            var buffer = reader.ReadBytes(HeaderSize);
            return new ResourcesHeader(buffer);
        }
    }
}