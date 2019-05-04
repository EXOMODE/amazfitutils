using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Resources.Models
{
    public class Image: IResource
    {
        public Bitmap Bitmap { get; }

        public Image(Bitmap bitmap)
        {
            Bitmap = bitmap;
        }

        public static string ResourceExtension = ".png";
        public string Extension => ResourceExtension;

        public void WriteTo(Stream stream)
        {
            new Resources.Image.Writer(stream).Write(Bitmap);
        }

        public void ExportTo(Stream stream)
        {
            Bitmap.Save(stream, ImageFormat.Png);
        }
    }
}