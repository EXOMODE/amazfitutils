using System;
using System.Collections.Generic;
using System.IO;
using SixLabors.ImageSharp;
using WatchFace.Models;

namespace WatchFace
{
    public class Reader
    {
        private readonly FileStream _fileStream;

        public Reader(FileStream streamReader)
        {
            _fileStream = streamReader;
        }

        public List<Parameter> Resources { get; private set; }
        public List<Image<Rgba32>> Images { get; private set; }

        public void Parse()
        {
            var header = Header.ReadFrom(_fileStream);
            if (!header.IsValid) return;

            var parametersStream = StreamBlock(_fileStream, (int) header.ParametersSize);

            var mainParam = Parameter.ReadFrom(parametersStream);
            var parameters = Parameter.ReadList(parametersStream);
            var coordinatesTableSize = mainParam.Children[0].Value;
            var imagesTableLength = mainParam.Children[1].Value;

            ParseResources(coordinatesTableSize, parameters);
            ParseImages(imagesTableLength);
        }

        private void ParseResources(long coordinatesTableSize, IReadOnlyCollection<Parameter> parameters)
        {
            var coordsStream = StreamBlock(_fileStream, (int) coordinatesTableSize);

            Resources = new List<Parameter>(parameters.Count);
            foreach (var parameter in parameters)
            {
                var descriptorOffset = parameter.Children[0].Value;
                var descriptorLength = parameter.Children[1].Value;
                coordsStream.Seek(descriptorOffset, SeekOrigin.Begin);
                var descriptorStream = StreamBlock(coordsStream, (int) descriptorLength);
                Resources.Add(new Parameter(parameter.Id, Parameter.ReadList(descriptorStream)));
            }
        }

        private void ParseImages(long imagesTableLength)
        {
            var imagesOffsets = new byte[imagesTableLength * 4];
            _fileStream.Read(imagesOffsets, 0, imagesOffsets.Length);

            var imagesOffset = _fileStream.Position;

            Images = new List<Image<Rgba32>>((int) imagesTableLength);
            for (var i = 0; i < (int) imagesTableLength; i++)
            {
                var imageOffset = BitConverter.ToUInt32(imagesOffsets, i * 4) + imagesOffset;
                _fileStream.Seek(imageOffset, SeekOrigin.Begin);
                var image = new ImageReader(_fileStream).Read();
                Images.Add(image);
            }
        }

        private static Stream StreamBlock(Stream stream, int size)
        {
            var buffer = new byte[size];
            stream.Read(buffer, 0, buffer.Length);
            return new MemoryStream(buffer);
        }
    }
}