using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using NLog;
using Resources.Image;

namespace Resources
{
    public class ImageLoader
    {
        public static readonly int NumericPartLength = 4;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static Bitmap LoadImageForNumber(string directory, long index)
        {
            var numericParts = new[]
            {
                index.ToString().PadLeft(4, '0'),
                index.ToString().PadLeft(3, '0'),
                index.ToString().PadLeft(2, '0'),
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

                var image = (Bitmap) System.Drawing.Image.FromFile(fullFileName);
                Logger.Trace("Image was loaded from file {0}", fullFileName);
                return ApplyDithering(image);
            }

            throw new FileNotFoundException($"File referenced by index {index} not found.");
        }

        private static Bitmap ApplyDithering(Bitmap image)
        {
            var clone = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            using (var gr = Graphics.FromImage(clone))
            {
                gr.DrawImage(image, new Rectangle(0, 0, clone.Width, clone.Height));
            }

            FloydSteinbergDitherer.Process(clone);
            return clone;
        }
    }
}