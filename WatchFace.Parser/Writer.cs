using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NLog;
using WatchFace.Parser.Models;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser
{
    public class Writer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly List<Bitmap> _images;

        private readonly Stream _stream;

        public Writer(Stream stream, List<Bitmap> images)
        {
            _stream = stream;
            _images = images;
        }

        public void Write(WatchFace watchFace)
        {
            Logger.Trace("Building parameters for watch face...");
            var descriptor = ParametersConverter.Build(watchFace);

            Logger.Trace("Encoding parameters...");
            var encodedParameters = new Dictionary<byte, MemoryStream>(descriptor.Count);
            foreach (var parameter in descriptor)
            {
                var memoryStream = new MemoryStream();
                foreach (var child in parameter.Children)
                    child.Write(memoryStream);
                encodedParameters[parameter.Id] = memoryStream;
            }

            Logger.Trace("Encoding offsets and lengths...");
            var parametersPositions = new List<Parameter>(descriptor.Count + 1);
            var offset = (long) 0;
            foreach (var encodedParameter in encodedParameters)
            {
                var encodedParameterId = encodedParameter.Key;
                var encodedParameterLength = encodedParameter.Value.Length;
                parametersPositions.Add(new Parameter(encodedParameterId, new List<Parameter>
                {
                    new Parameter(1, offset),
                    new Parameter(2, encodedParameterLength)
                }));
                offset += encodedParameterLength;
            }
            parametersPositions.Insert(0, new Parameter(1, new List<Parameter>
            {
                new Parameter(1, offset),
                new Parameter(2, _images.Count)
            }));

            var encodedParametersPositions = new MemoryStream();
            foreach (var parametersPosition in parametersPositions)
                parametersPosition.Write(encodedParametersPositions);

            Logger.Trace("Writing header...");
            var header = new Header
            {
                ParametersSize = (uint) encodedParametersPositions.Length,
                Unknown = 0x159 // Maybe some kind of layers (the bigger number needed for more complex watch faces)
            };
            header.WriteTo(_stream);

            Logger.Trace("Writing parameters offsets and lengths...");
            encodedParametersPositions.Seek(0, SeekOrigin.Begin);
            encodedParametersPositions.WriteTo(_stream);

            Logger.Trace("Writing parameters...");
            foreach (var encodedParameter in encodedParameters)
            {
                var stream = encodedParameter.Value;
                stream.Seek(0, SeekOrigin.Begin);
                stream.WriteTo(_stream);
            }
            Logger.Trace("Writing images...");
            new Resources.Writer(_stream).Write(_images);
        }
    }
}