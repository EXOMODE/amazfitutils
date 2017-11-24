using System;
using System.Drawing;
using System.IO;
using NLog;

namespace Resources
{
    public class ResourcesReader
    {
        private const int OffsetTableItemLength = 4;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly BinaryReader _binaryReader;
        private readonly Stream _stream;

        public ResourcesReader(Stream stream)
        {
            _stream = stream;
            _binaryReader = new BinaryReader(_stream);
        }

        public Bitmap[] Read(uint imagesTableLength)
        {
            var offsetsTableLength = (int) (imagesTableLength * OffsetTableItemLength);
            Logger.Trace("Reading resources offsets table with {0} elements ({1} bytes)",
                imagesTableLength, offsetsTableLength
            );
            var imagesOffsets = _binaryReader.ReadBytes(offsetsTableLength);
            var imagesOffset = _stream.Position;

            Logger.Trace("Reading {0} resources", imagesTableLength);
            var images = new Bitmap[imagesTableLength];
            for (var i = 0; i < imagesTableLength; i++)
            {
                var imageOffset = BitConverter.ToUInt32(imagesOffsets, i * OffsetTableItemLength) + imagesOffset;
                if (_stream.Position != imageOffset)
                {
                    var bytesGap = imageOffset - _stream.Position;
                    Logger.Warn("Found {0} bytes gap before resource number {1}", bytesGap, i);
                    _stream.Seek(imageOffset, SeekOrigin.Begin);
                }
                images[i] = new ImageReader(_stream).Read();
            }
            return images;
        }
    }
}