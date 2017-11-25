using System;
using System.Drawing;
using System.IO;
using NLog;

namespace Resources
{
    public class ImageReader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly BinaryReader _reader;
        private ushort _bitsPerPixel;
        private ushort _height;
        private Color[] _palette;
        private ushort _paletteColors;
        private ushort _rowLengthInBytes;
        private ushort _transparency;
        private ushort _width;

        public ImageReader(Stream stream)
        {
            _reader = new BinaryReader(stream);
        }

        public Bitmap Read()
        {
            var signature = _reader.ReadChars(4);
            if (signature[0] != 'B' || signature[1] != 'M')
                throw new ArgumentException("Image signature doesn't match.");

            ReadHeader();
            ReadPalette();
            return ReadImage();
        }

        private void ReadHeader()
        {
            Logger.Trace("Reading image header...");
            _width = _reader.ReadUInt16();
            _height = _reader.ReadUInt16();
            _rowLengthInBytes = _reader.ReadUInt16();
            _bitsPerPixel = _reader.ReadUInt16();
            _paletteColors = _reader.ReadUInt16();
            _transparency = _reader.ReadUInt16();
            Logger.Trace("Image header was read:");
            Logger.Trace("Width: {0}, Height: {1}, RowLength: {2}", _width, _height, _rowLengthInBytes);
            Logger.Trace("BPP: {0}, PaletteColors: {1}, Transaparency: {2}",
                _bitsPerPixel, _paletteColors, _transparency
            );
        }

        private void ReadPalette()
        {
            Logger.Trace("Reading palette...");
            _palette = new Color[_paletteColors];
            for (var i = 0; i < _paletteColors; i++)
            {
                var r = _reader.ReadByte();
                var g = _reader.ReadByte();
                var b = _reader.ReadByte();
                var a = _reader.ReadByte();
                Logger.Trace("Palette item {0}: R {1:x2}, G {2:x2}, B {3:x2}, A {4:x2}", i, r, g, b, a);

                var color = _transparency > 0 && i == 0 ? Color.Transparent : Color.FromArgb(0xff, r, g, b);

                _palette[i] = color;
            }
        }

        private Bitmap ReadImage()
        {
            var image = new Bitmap(_width, _height);

            for (var i = 0; i < _height; i++)
            {
                var row = _reader.ReadBytes(_rowLengthInBytes);
                var bitReader = new BitReader(row);
                for (var j = 0; j < _width; j++)
                {
                    var pixelColorIndex = bitReader.ReadBits(_bitsPerPixel);
                    var color = _palette[(int) pixelColorIndex];
                    image.SetPixel(j, i, color);
                }
            }
            return image;
        }
    }
}