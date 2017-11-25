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
        private bool _transparency;
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
            _transparency = _reader.ReadUInt16() > 0;
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
                var padding = _reader.ReadByte(); // always 0 maybe padding

                if (padding != 0) Logger.Warn("Palette item {0} last byte is not zero: {1:X2}", i, padding);

                var isColorValid = (r == 0 || r == 0xff) && (g == 0 || g == 0xff) && (b == 0 || b == 0xff);

                if (isColorValid)
                    Logger.Trace("Palette item {0}: R {1:X2}, G {2:X2}, B {3:X2}", i, r, g, b);
                else
                    Logger.Warn("Palette item {0}: R {1:X2}, G {2:X2}, B {3:X2}, color isn't supported!", i, r, g, b);

                _palette[i] = Color.FromArgb(_transparency && i == 0 ? 0x00 : 0xff, r, g, b);
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