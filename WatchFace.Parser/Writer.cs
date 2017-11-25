using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NLog;
using Resources;
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
            var descriptor = ParametersConverter.Build(watchFace);
            var encodedParameters = new Dictionary<byte, MemoryStream>(descriptor.Count);
            foreach (var parameter in descriptor)
            {
                var memoryStream = new MemoryStream();
                foreach (var child in parameter.Children)
                    child.Write(memoryStream);
                encodedParameters[parameter.Id] = memoryStream;
            }

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

            var header = new Header
            {
                ParametersSize = (uint) encodedParametersPositions.Length,
                Unknown = 0x37 // Value from Sydney (watchface with 0 doesn't work)
            };
            header.WriteTo(_stream);

            encodedParametersPositions.Seek(0, SeekOrigin.Begin);
            encodedParametersPositions.WriteTo(_stream);

            foreach (var encodedParameter in encodedParameters)
            {
                var stream = encodedParameter.Value;
                stream.Seek(0, SeekOrigin.Begin);
                stream.WriteTo(_stream);
            }
            WriteImages();
        }

        public void WriteImages()
        {
            var offsetsTable = new byte[_images.Count * 4];
            var encodedImages = new MemoryStream[_images.Count];

            var offset = (uint) 0;
            for (var i = 0; i < _images.Count; i++)
            {
                var offsetBytes = BitConverter.GetBytes(offset);
                offsetBytes.CopyTo(offsetsTable, i * 4);

                var encodedImage = new MemoryStream();
                Logger.Debug("Writing image {0}...", i);
                new ImageWriter(encodedImage, _images[i]).Write();
                offset += (uint) encodedImage.Length;
                encodedImages[i] = encodedImage;
            }

            _stream.Write(offsetsTable, 0, offsetsTable.Length);

            foreach (var encodedImage in encodedImages)
            {
                encodedImage.Seek(0, SeekOrigin.Begin);
                encodedImage.CopyTo(_stream);
            }
        }
    }
}