using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Fonts.Models;
using NLog;

namespace Fonts
{
    public class FileReader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static FileDescriptor Read(Stream stream)
        {
            var binaryReader = new BinaryReader(stream);

            var signature = Encoding.ASCII.GetString(binaryReader.ReadBytes(4));
            Logger.Trace("Font signature was read:");
            stream.Seek(0, SeekOrigin.Begin);
            Logger.Trace("Signature: {0}", signature);

            NewHeader header;
            switch (signature) {
                case NewHeader.ResSignature:
                    header = NewHeader.ReadFrom(binaryReader);
                    break;
                default:
                    throw new ArgumentException($"Signature '{signature}' is no recognized.");
            }

            Logger.Trace("Font header was read:");
            Logger.Trace("Version: {0}, BlocksCount: {1}", header.Version, header.BlocksCount);

            var blocks = new Reader(stream).Read(header.BlocksCount);
            return new FileDescriptor
            {
                HasNewHeader = true,
                BlocksCount = header.BlocksCount,
                Version = header.Version,
                Unknown = header.Unknown,
                Blocks = blocks,
                Images = new Reader(stream).Read(blocks)
            };
        }
    }
}