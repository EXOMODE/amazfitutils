using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using SixLabors.ImageSharp;
using WatchFace.Models;

namespace WatchFace
{
    public class Reader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly FileStream _fileStream;

        public Reader(FileStream streamReader)
        {
            _fileStream = streamReader;
        }

        public List<Parameter> Resources { get; private set; }
        public List<Image<Rgba32>> Images { get; private set; }

        public void Read()
        {
            Logger.Trace("Reading header...");
            var header = Header.ReadFrom(_fileStream);
            Logger.Trace("Header was read:");
            Logger.Trace("Signature: {0}, Unknown: {1}, ParametersSize: {2}, IsValid: {3}", header.Signature,
                header.Unknown,
                header.ParametersSize, header.IsValid);
            if (!header.IsValid) return;

            Logger.Trace("Reading parameter offsets...");
            var parametersStream = StreamBlock(_fileStream, (int) header.ParametersSize);
            Logger.Trace("Parameter offsets were read from file");

            Logger.Trace("Reading parameters descriptor...");
            var mainParam = Parameter.ReadFrom(parametersStream);
            Logger.Trace("Parameters descriptor was read:");
            var coordinatesTableSize = mainParam.Children[0].Value;
            var imagesTableLength = mainParam.Children[1].Value;
            Logger.Trace($"CoordinatesTableSize: {coordinatesTableSize}, ImagesTableLength: {imagesTableLength}");

            Logger.Trace("Reading face parameters...");
            var parameters = Parameter.ReadList(parametersStream);
            Logger.Trace("Watch face parameters were read:");

            ParseResources(coordinatesTableSize, parameters);
            ParseImages(imagesTableLength);
        }

        private void ParseResources(long coordinatesTableSize, IReadOnlyCollection<Parameter> parameters)
        {
            var coordsStream = StreamBlock(_fileStream, (int) coordinatesTableSize);

            Resources = new List<Parameter>(parameters.Count);
            foreach (var parameter in parameters)
            {
                Logger.Trace("Reading descriptor for parameter {0}", parameter.Id);
                var descriptorOffset = parameter.Children[0].Value;
                var descriptorLength = parameter.Children[1].Value;
                Logger.Trace("Descriptor offset: {0}, Descriptor length: {1}", descriptorOffset, descriptorLength);
                coordsStream.Seek(descriptorOffset, SeekOrigin.Begin);
                var descriptorStream = StreamBlock(coordsStream, (int) descriptorLength);
                Logger.Trace("Parsing descriptor for parameter {0}...", parameter.Id);
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