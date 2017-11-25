using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using NLog;

namespace Resources
{
    public class ResourcesExtractor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Bitmap[] _images;

        public ResourcesExtractor(Bitmap[] images)
        {
            _images = images;
        }

        public void Extract(string outputDirectory)
        {
            for (var i = 0; i < _images.Length; i++)
            {
                var fileName = Path.Combine(outputDirectory, $"{i}.png");
                Logger.Debug("Exporting {0}...", fileName);
                _images[i].Save(fileName, ImageFormat.Png);
            }
        }
    }
}