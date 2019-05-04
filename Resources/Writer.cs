using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NLog;
using Resources.Models;

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

        public void Write(List<IResource> resources)
        {
            var offsetsTable = new byte[resources.Count * OffsetTableItemLength];
            var encodedResources = new MemoryStream[resources.Count];

            var offset = (uint) 0;
            for (var i = 0; i < resources.Count; i++)
            {
                Logger.Trace("Resource {0} offset is {1}...", i, offset);
                var offsetBytes = BitConverter.GetBytes(offset);
                offsetBytes.CopyTo(offsetsTable, i * OffsetTableItemLength);

                var encodedImage = new MemoryStream();
                Logger.Debug("Encoding resource {0}...", i);
                resources[i].WriteTo(encodedImage);
                offset += (uint) encodedImage.Length;
                encodedResources[i] = encodedImage;
            }

            Logger.Trace("Writing resources offsets table");
            _stream.Write(offsetsTable, 0, offsetsTable.Length);

            Logger.Debug("Writing resources");
            foreach (var encodedImage in encodedResources)
            {
                encodedImage.Seek(0, SeekOrigin.Begin);
                encodedImage.CopyTo(_stream);
            }
        }
    }
}