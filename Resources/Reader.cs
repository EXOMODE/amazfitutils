using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NLog;

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

        public List<Bitmap> Read(uint imagesTableLength)
        {
            var offsetsTableLength = (int) (imagesTableLength * OffsetTableItemLength);
            Logger.Trace("Reading resources offsets table with {0} elements ({1} bytes)",
                imagesTableLength, offsetsTableLength
            );
            var imagesOffsets = _binaryReader.ReadBytes(offsetsTableLength);
            var imagesOffset = _stream.Position;

            Logger.Debug("Reading {0} images...", imagesTableLength);
            var images = new List<Bitmap>((int) imagesTableLength);
            for (var i = 0; i < imagesTableLength; i++)
            {
                var imageOffset = BitConverter.ToUInt32(imagesOffsets, i * OffsetTableItemLength);
                var realOffset = imageOffset + imagesOffset;
                Logger.Trace("Image {0} offset is {1}...", i, imageOffset);
                if (_stream.Position != realOffset)
                {
                    var bytesGap = realOffset - _stream.Position;
                    Logger.Warn("Found {0} bytes gap before resource number {1}", bytesGap, i);
                    _stream.Seek(realOffset, SeekOrigin.Begin);
                }
                Logger.Debug("Reading image {0}...", i);
                try
                {
                    images.Add(new Image.Reader(_stream).Read());
                }
                catch
                {
                    Logger.Warn("Error on reading image {0}", i);
                }
            }
            return images;
        }
    }
}