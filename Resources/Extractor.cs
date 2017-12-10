using System.Drawing.Imaging;
using System.IO;
using NLog;
using Resources.Models;

namespace Resources
{
    public class Extractor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly FileDescriptor _descriptor;

        public Extractor(FileDescriptor descriptor)
        {
            _descriptor = descriptor;
        }

        public void Extract(string outputDirectory)
        {
            if (_descriptor.Version != null)
            {
                var fileName = Path.Combine(outputDirectory, "version");
                using (var stream = File.OpenWrite(fileName))
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(_descriptor.Version.Value);
                }
            }

            for (var i = 0; i < _descriptor.Images.Count; i++)
            {
                var numericPart = i.ToString().PadLeft(ImageLoader.NumericPartLength, '0');
                var fileName = Path.Combine(outputDirectory, numericPart + ".png");
                Logger.Debug("Extracting {0}...", fileName);
                _descriptor.Images[i].Save(fileName, ImageFormat.Png);
            }
        }
    }
}