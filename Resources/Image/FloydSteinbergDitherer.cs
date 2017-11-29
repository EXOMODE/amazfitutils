using System.Drawing;
using NLog;

namespace Resources.Image
{
    public class FloydSteinbergDitherer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Process(Bitmap image)
        {
            var isImageAltered = false;
            for (var y = 0; y < image.Height; y++)
            for (var x = 0; x < image.Width; x++)
            {
                var color = image.GetPixel(x, y);
                var colorError = new ColorError(color);

                if (colorError.IsZero) continue;

                if (!isImageAltered)
                {
                    Logger.Warn(
                        "Dithering applied for an image. Resource in watch face will use only supported colors. You can't get back original image by unpacking watch face."
                    );
                    isImageAltered = true;
                }

                image.SetPixel(x, y, colorError.NewColor);

                if (x + 1 < image.Width)
                {
                    color = image.GetPixel(x + 1, y);
                    image.SetPixel(x + 1, y, colorError.ApplyError(color, 7, 16));
                }

                if (y + 1 < image.Height)
                {
                    if (x > 1)
                    {
                        color = image.GetPixel(x - 1, y + 1);
                        image.SetPixel(x - 1, y + 1, colorError.ApplyError(color, 3, 16));
                    }

                    color = image.GetPixel(x, y + 1);
                    image.SetPixel(x, y + 1, colorError.ApplyError(color, 5, 16));

                    if (x < image.Width - 1)
                    {
                        color = image.GetPixel(x + 1, y + 1);
                        image.SetPixel(x + 1, y + 1, colorError.ApplyError(color, 1, 16));
                    }
                }
            }
        }
    }
}