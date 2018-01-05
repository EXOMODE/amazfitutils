using System;
using System.Drawing;
using System.IO;
using BumpKit;
using NLog;
using Resources.Utils;

namespace Resources.Image
{
    public class Reader
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

        public Reader(Stream stream)
        {
            _reader = new BinaryReader(stream);
        }

        public Bitmap Read()
        {
            var signature = _reader.ReadChars(4);
            if (signature[0] != 'B' || signature[1] != 'M')
                throw new ArgumentException("Image signature doesn't match.");

            ReadHeader();
            if (_paletteColors > 0)
                ReadPalette();
            else if (_bitsPerPixel == 16 || _bitsPerPixel == 24 || _bitsPerPixel == 32)
                Logger.Debug("The image doesn't use a palette.");
            else
                throw new ArgumentException("The image format is not supported. Please report the issue on https://bitbucket.org/valeronm/amazfitbiptools");
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

                var alpha = _transparency && i == 0 ? 0x00 : 0xff;
                _palette[i] = Color.FromArgb(alpha, r, g, b);
            }
        }

        private Bitmap ReadImage()
        {
            var image = new Bitmap(_width, _height);
            using (var context = image.CreateUnsafeContext())
            {
                for (var y = 0; y < _height; y++)
                {
                    var rowBytes = _reader.ReadBytes(_rowLengthInBytes);
                    var bitReader = new BitReader(rowBytes);
                    for (var x = 0; x < _width; x++)
                    {
                        if (_paletteColors > 0)
                        {
                            var pixelColorIndex = bitReader.ReadBits(_bitsPerPixel);
                            var color = _palette[(int) pixelColorIndex];
                            context.SetPixel(x, y, color);
                        }
                        else if (_bitsPerPixel == 16)
                        {
                            var firstByte = bitReader.ReadByte();
                            var secondByte = bitReader.ReadByte();
                            var b = (secondByte >> 3 & 0x1f) << 3;
                            var g = (secondByte & 0x07 << 2 | firstByte & 0x40 >> 5) << 3;
                            var r = (firstByte & 0x1f) << 3;
                            var color = Color.FromArgb(0xff, r, g, b);
                            context.SetPixel(x, y, color);
                        }
                        else if (_bitsPerPixel == 24)
                        {
                            var alpha = (int)bitReader.ReadByte();
                            var b = (int)(bitReader.ReadBits(5) << 3);
                            var g = (int)(bitReader.ReadBits(6) << 2);
                            var r = (int)(bitReader.ReadBits(5) << 3);
                            var color = Color.FromArgb(0xff - alpha, r, g, b);
                            context.SetPixel(x, y, color);
                        }
                        else if (_bitsPerPixel == 32)
                        {
                            var alpha = (int)bitReader.ReadByte();
                            var b = (int)bitReader.ReadByte();
                            var g = (int)bitReader.ReadByte();
                            var r = (int)bitReader.ReadByte();
                            var color = Color.FromArgb(0xff - alpha, r, g, b);
                            context.SetPixel(x, y, color);
                        }
                    }
                }
            }
            return image;
        }
    }
}