﻿using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NLog;
using WatchFace.Parser.Utils;
using WatchFace.Parser.Models;

namespace WatchFace.Parser
{
    public class Writer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Stream _stream;
        private readonly List<Bitmap> _images;

        public Writer(Stream stream, List<Bitmap> images)
        {
            _stream = stream;
            _images = images;
        }

        public void Write(WatchFace watchFace)
        {
            var descriptor = ParametersConverter.Build(watchFace);
            var encodedParameters = new Dictionary<long, MemoryStream>(descriptor.Count);
            foreach (var parameter in descriptor)
            {
                var memoryStream = new MemoryStream();
                parameter.Write(memoryStream);
                encodedParameters[parameter.Id] = memoryStream;
            }

            var header = new Header {ParametersSize = 20};
            header.WriteTo(_stream);
        }
    }
}