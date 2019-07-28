using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using NLog;
using Resources.Image;
using Resources.Models;

namespace Resources
{
    public class ImageLoader
    {
        public static readonly int NumericPartLength = 4;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IResource LoadResourceForNumber(string directory, long index)
        {
            var strIndex = index.ToString();
            var numericParts = new[]
            {
                strIndex.PadLeft(4, '0'), strIndex.PadLeft(3, '0'), strIndex.PadLeft(2, '0'), strIndex
            }.Distinct();

            foreach (var numericPart in numericParts)
            {
                var resource = TryLoadBitmap(directory, numericPart);
                if (resource != null) return resource;

                resource = TryLoadBlob(directory, numericPart);
                if (resource != null) return resource;
            }

            return default;
            //throw new FileNotFoundException($"File referenced by index {index} not found.");
        }

        private static IResource TryLoadBitmap(string directory, string numericPart)
        {
            var fullFileName = Path.Combine(directory, numericPart + Models.Image.ResourceExtension);
            if (!File.Exists(fullFileName))
            {
                Logger.Trace("File {0} doesn't exists.", fullFileName);
                return null;
            }

            var image = (Bitmap) System.Drawing.Image.FromFile(fullFileName);
            Logger.Trace("Image was loaded from file {0}", fullFileName);
            var ditheredBitmap = ApplyDithering(image);
            return new Models.Image(ditheredBitmap);
        }

        private static IResource TryLoadBlob(string directory, string numericPart)
        {
            var fullFileName = Path.Combine(directory, numericPart + Blob.ResourceExtension);
            if (!File.Exists(fullFileName))
            {
                Logger.Trace("File {0} doesn't exists.", fullFileName);
                return null;
            }

            using (var fileStream = File.OpenRead(fullFileName))
            {
                var data = new byte[fileStream.Length];
                fileStream.Read(data, 0, data.Length);
                return new Blob(data);
            }
        }

        private static Bitmap ApplyDithering(Bitmap image)
        {
            var clone = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            using (var gr = Graphics.FromImage(clone))
            {
                gr.DrawImage(image, new Rectangle(0, 0, clone.Width, clone.Height));
            }

            //if (!Image.Reader.IsVerge) FloydSteinbergDitherer.Process(clone);

            return clone;
        }
    }
}