using System.Drawing;
using System.IO;
using BumpKit;
using FwFonts.Models;
using Utils;

namespace FwFonts
{
    public class GlyphReader
    {
        private readonly Stream _stream;
        private readonly BinaryReader _reader;

        public GlyphReader(Stream stream)
        {
            _stream = stream;
            _reader = new BinaryReader(stream);
        }

        public Bitmap Read(GlyphDescriptor glyph)
        {
            _stream.Seek(glyph.DataFileOffset, SeekOrigin.Begin);
            var image = new Bitmap(glyph.Width + glyph.OffsetX, glyph.Height + glyph.OffsetY);
            using (var context = image.CreateUnsafeContext())
            {
                for (var y = glyph.OffsetY; y < image.Height; y++)
                {
                    var rowBytes = _reader.ReadBytes(glyph.RowLength);
                    var bitReader = new BitReader(rowBytes);
                    for (var x = glyph.OffsetX; x < image.Width; x++)
                    {
                        var pixelValue = bitReader.ReadBits(1);
                        var color = pixelValue == 1 ? Color.Black : Color.FromArgb(0, Color.White);
                        context.SetPixel(x, y, color);
                    }
                }
            }
            return image;
        }
    }
}