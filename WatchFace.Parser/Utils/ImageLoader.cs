using System.Drawing;
using System.IO;
using NLog;
using Resources;

namespace WatchFace.Parser.Utils
{
    public class ImageLoader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static Bitmap LoadImageForNumber(string directory, long index)
        {
            var numericParts = new[] {index.ToString().PadLeft(Extractor.NumericPartLength, '0'), index.ToString()};

            foreach (var numericPart in numericParts)
            {
                var fullFileName = Path.Combine(directory, numericPart + ".png");
                if (!File.Exists(fullFileName))
                {
                    Logger.Trace("File {0} doesn't exists.", fullFileName);
                    continue;
                }

                Logger.Trace("Image was loaded from file {0}", fullFileName);
                return (Bitmap) Image.FromFile(fullFileName);
            }

            throw new FileNotFoundException($"File referenced by index {index} not found.");
        }
    }
}