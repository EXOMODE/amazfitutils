using System.Drawing;
using System.IO;
using System.Linq;
using NLog;

namespace Resources
{
    public class ImageLoader
    {
        public static readonly int NumericPartLength = 3;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static Bitmap LoadImageForNumber(string directory, long index)
        {
            var numericParts = new[]
            {
                index.ToString().PadLeft(NumericPartLength, '0'),
                index.ToString()
            };

            foreach (var numericPart in numericParts.Distinct())
            {
                var fullFileName = Path.Combine(directory, numericPart + ".png");
                if (!File.Exists(fullFileName))
                {
                    Logger.Trace("File {0} doesn't exists.", fullFileName);
                    continue;
                }

                Logger.Trace("Image was loaded from file {0}", fullFileName);
                return (Bitmap) System.Drawing.Image.FromFile(fullFileName);
            }

            throw new FileNotFoundException($"File referenced by index {index} not found.");
        }
    }
}