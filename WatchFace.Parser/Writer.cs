using System.Collections.Generic;
using System.IO;
using NLog;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser
{
    public class Writer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly FileStream _fileStream;

        public Writer(FileStream streamReader)
        {
            _fileStream = streamReader;
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
        }
    }
}