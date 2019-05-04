using System.Collections.Generic;
using System.IO;
using NLog;
using Resources.Models;

namespace Resources
{
    public class Reader
    {
        private const int OffsetTableItemLength = 4;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly BinaryReader _binaryReader;
        private readonly Stream _stream;

        public Reader(Stream stream)
        {
            _stream = stream;
            _binaryReader = new BinaryReader(_stream);
        }

        public List<IResource> Read(uint resourcesCount)
        {
            var offsetsTableLength = (int) (resourcesCount * OffsetTableItemLength);
            Logger.Trace("Reading resources offsets table with {0} elements ({1} bytes)",
                resourcesCount, offsetsTableLength
            );

            var offsets = new int[resourcesCount];
            for (var i = 0; i < resourcesCount; i++)
                offsets[i] = _binaryReader.ReadInt32();

            var resourcesOffset = (int) _stream.Position;
            var fileSize = (int) _stream.Length;

            Logger.Debug("Reading {0} resources...", resourcesCount);
            var resources = new List<IResource>((int) resourcesCount);
            for (var i = 0; i < resourcesCount; i++)
            {
                var offset = offsets[i] + resourcesOffset;
                var nextOffset = i + 1 < resourcesCount ? offsets[i + 1] + resourcesOffset : fileSize;
                var length = nextOffset - offset;
                Logger.Trace("Resource {0} offset: {1}, length: {2}...", i, offset, length);
                if (_stream.Position != offset)
                {
                    var bytesGap = offset - _stream.Position;
                    Logger.Warn("Found {0} bytes gap before resource number {1}", bytesGap, i);
                    _stream.Seek(offset, SeekOrigin.Begin);
                }

                Logger.Debug("Reading resource {0}...", i);
                try
                {
                    var bitmap = new Image.Reader(_stream).Read();
                    var image = new Models.Image(bitmap);
                    resources.Add(image);
                }
                catch (InvalidResourceException)
                {
                    Logger.Warn("Resource is not an image");
                    _stream.Seek(offset, SeekOrigin.Begin);
                    var data = new byte[length];
                    _stream.Read(data, 0, length);
                    var blob = new Blob(data);
                    resources.Add(blob);
                }
            }

            return resources;
        }
    }
}