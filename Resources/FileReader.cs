using System;
using System.IO;
using System.Text;
using NLog;
using Resources.Models;

namespace Resources
{
    public class FileReader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static FileDescriptor Read(Stream stream)
        {
            var binaryReader = new BinaryReader(stream);

            var signature = Encoding.ASCII.GetString(binaryReader.ReadBytes(5));
            Logger.Trace("Resources signature was read:");
            stream.Seek(0, SeekOrigin.Begin);
            Logger.Trace("Signature: {0}", signature);

            Header header;
            switch (signature) {
                case Header.ResSignature:
                    header = Header.ReadFrom(binaryReader);
                    break;
                case NewHeader.ResSignature:
                    header = NewHeader.ReadFrom(binaryReader);
                    break;
                default:
                    throw new ArgumentException($"Signature '{signature}' is no recognized.");
            }

            Logger.Trace("Resources header was read:");
            Logger.Trace("Version: {0}, ResourcesCount: {1}", header.Version, header.ResourcesCount);

            return new FileDescriptor
            {
                HasNewHeader = header is NewHeader,
                ResourcesCount = header.ResourcesCount,
                Version = header.Version,
                Unknown = (header as NewHeader)?.Unknown,
                Resources = new Reader(stream).Read(header.ResourcesCount)
            };
        }
    }
}