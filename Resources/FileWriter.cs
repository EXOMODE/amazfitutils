using System;
using System.IO;
using NLog;
using Resources.Models;

namespace Resources
{
    public class FileWriter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly BinaryWriter _binaryWriter;
        private readonly Stream _stream;

        public FileWriter(Stream stream)
        {
            _stream = stream;
            _binaryWriter = new BinaryWriter(stream);
        }

        public void Write(FileDescriptor descriptor)
        {
            if (descriptor.Version == null)
                throw new ArgumentException("Res file version required");

            var header = new Header
            {
                ResourcesCount = (uint) descriptor.Images.Count,
                Version = descriptor.Version.Value
            };
            Logger.Trace("Writing resources header...");
            Logger.Trace("Signature: {0}, Version: {1}, ResourcesCount: {2}, IsValid: {3}",
                header.Signature, header.Version, header.ResourcesCount, header.IsValid
            );
            header.WriteTo(_binaryWriter);

            Logger.Trace("Writing images...");
            new Writer(_stream).Write(descriptor.Images);
        }
    }
}