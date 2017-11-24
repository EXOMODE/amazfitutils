using System;
using System.Drawing;
using System.IO;
using NLog;

namespace Resources
{
    public class ResourcesFileReader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly BinaryReader _binaryReader;
        private readonly Stream _stream;

        public ResourcesFileReader(Stream stream)
        {
            _stream = stream;
            _binaryReader = new BinaryReader(stream);
        }

        public Bitmap[] Read()
        {
            Logger.Trace("Reading resources header");
            var header = ResourcesHeader.ReadFrom(_binaryReader);
            Logger.Trace("Resources header was read:");
            Logger.Trace("Signature: {0}, Version: {1}, ResourcesCount: {2}, IsValid: {3}",
                header.Signature, header.Version, header.ResourcesCount, header.IsValid
            );

            if (!header.IsValid)
                throw new ArgumentException("Invalid resources header");

            return new ResourcesReader(_stream).Read(header.ResourcesCount);
        }
    }
}