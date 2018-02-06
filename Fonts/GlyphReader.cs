using System.Drawing;
using System.IO;
using BumpKit;
using Utils;

namespace Fonts
{
    public class GlyphReader
    {
        private readonly BinaryReader _reader;

        public GlyphReader(Stream stream)
        {
            _reader = new BinaryReader(stream);
        }

        public Bitmap Read(int width, int height)
        {
            var image = new Bitmap(width, height);
            using (var context = image.CreateUnsafeContext())
            {
                for (var y = 0; y < height; y++)
                {
                    var rowBytes = _reader.ReadBytes(width / 8);
                    var bitReader = new BitReader(rowBytes);
                    for (var x = 0; x < width; x++)
                    {
                        var pixelValue = bitReader.ReadBits(1);
                        var color = pixelValue == 1 ? Color.Black : Color.White;
                        context.SetPixel(x, y, color);
                    }
                }
            }
            return image;
        }
    }
}