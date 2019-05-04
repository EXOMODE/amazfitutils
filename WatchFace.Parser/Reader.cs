using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NLog;
using Resources.Models;
using WatchFace.Parser.Models;
using Header = WatchFace.Parser.Models.Header;
using Image = Resources.Models.Image;

namespace WatchFace.Parser
{
    public class Reader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Stream _stream;

        public Reader(Stream stream)
        {
            _stream = stream;
        }

        public List<Parameter> Parameters { get; private set; }
        public List<IResource> Resources { get; private set; }
        public Bitmap[] Images => Resources.OfType<Image>().Select(i => i.Bitmap).ToArray();

        public void Read()
        {
            Logger.Trace("Reading header...");
            var header = Header.ReadFrom(_stream);
            Logger.Trace("Header was read:");
            Logger.Trace("Signature: {0}, Unknown: {1}, ParametersSize: {2}, IsValid: {3}", header.Signature,
                header.Unknown,
                header.ParametersSize, header.IsValid);
            if (!header.IsValid) return;

            Logger.Trace("Reading parameter offsets...");
            var parametersStream = StreamBlock(_stream, (int) header.ParametersSize);
            Logger.Trace("Parameter offsets were read from file");

            Logger.Trace("Reading parameters descriptor...");
            var mainParam = Parameter.ReadFrom(parametersStream);
            Logger.Trace("Parameters descriptor was read:");
            var parametrsTableLength = mainParam.Children[0].Value;
            var imagesCount = mainParam.Children[1].Value;
            Logger.Trace($"ParametrsTableLength: {parametrsTableLength}, ImagesCount: {imagesCount}");

            Logger.Trace("Reading parameters locations...");
            var parametersLocations = Parameter.ReadList(parametersStream);
            Logger.Trace("Watch face parameters locations were read:");

            Parameters = ReadParameters(parametrsTableLength, parametersLocations);
            Resources = new Resources.Reader(_stream).Read((uint) imagesCount);
        }

        private List<Parameter> ReadParameters(long coordinatesTableSize, ICollection<Parameter> parametersDescriptors)
        {
            var parametersStream = StreamBlock(_stream, (int) coordinatesTableSize);

            var result = new List<Parameter>(parametersDescriptors.Count);
            foreach (var prameterDescriptor in parametersDescriptors)
            {
                var descriptorOffset = prameterDescriptor.Children[0].Value;
                var descriptorLength = prameterDescriptor.Children[1].Value;
                Logger.Trace("Reading descriptor for parameter {0}", prameterDescriptor.Id);
                Logger.Trace("Descriptor offset: {0}, Descriptor length: {1}", descriptorOffset, descriptorLength);
                parametersStream.Seek(descriptorOffset, SeekOrigin.Begin);
                var descriptorStream = StreamBlock(parametersStream, (int) descriptorLength);
                Logger.Trace("Parsing descriptor for parameter {0}...", prameterDescriptor.Id);
                result.Add(new Parameter(prameterDescriptor.Id, Parameter.ReadList(descriptorStream)));
            }
            return result;
        }

        private static Stream StreamBlock(Stream stream, int size)
        {
            var buffer = new byte[size];
            stream.Read(buffer, 0, buffer.Length);
            return new MemoryStream(buffer);
        }
    }
}