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
            foreach (var parameter in descriptor) parameter.Write(_fileStream);
        }
    }
}