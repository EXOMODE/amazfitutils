using System;
using System.IO;
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

            Logger.Trace("Reading resources header");
            var header = Header.ReadFrom(binaryReader);
            Logger.Trace("Resources header was read:");
            Logger.Trace("Signature: {0}, Version: {1}, ResourcesCount: {2}, IsValid: {3}",
                header.Signature, header.Version, header.ResourcesCount, header.IsValid
            );

            if (!header.IsValid)
                throw new ArgumentException("Invalid resources header");

            return new FileDescriptor
            {
                Version = header.Version,
                Images = new Reader(stream).Read(header.ResourcesCount)
            };
        }
    }
}