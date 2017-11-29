using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NLog;

namespace Resources
{
    public class Writer
    {
        private const int OffsetTableItemLength = 4;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Stream _stream;

        public Writer(Stream stream)
        {
            _stream = stream;
        }

        public void Write(List<Bitmap> images)
        {
            var offsetsTable = new byte[images.Count * OffsetTableItemLength];
            var encodedImages = new MemoryStream[images.Count];

            var offset = (uint) 0;
            for (var i = 0; i < images.Count; i++)
            {
                Logger.Trace("Image {0} offset is {1}...", i, offset);
                var offsetBytes = BitConverter.GetBytes(offset);
                offsetBytes.CopyTo(offsetsTable, i * OffsetTableItemLength);

                var encodedImage = new MemoryStream();
                Logger.Debug("Encoding image {0}...", i);
                new Image.Writer(encodedImage).Write(images[i]);
                offset += (uint) encodedImage.Length;
                encodedImages[i] = encodedImage;
            }

            Logger.Trace("Writing images offsets table");
            _stream.Write(offsetsTable, 0, offsetsTable.Length);

            Logger.Debug("Writing images");
            foreach (var encodedImage in encodedImages)
            {
                encodedImage.Seek(0, SeekOrigin.Begin);
                encodedImage.CopyTo(_stream);
            }
        }
    }
}