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

        public static bool IsVerge { get; set; }
        public static bool IsInverted { get; set; }

        public Reader(Stream stream)
        {
            _reader = new BinaryReader(stream);
        }

        public Bitmap Read()
        {
            var signature = _reader.ReadChars(4);
            if (signature[0] != 'B' || signature[1] != 'M')
                throw new InvalidResourceException("Image signature doesn't match.");

            ReadHeader();
            if (_paletteColors > 256)
                throw new InvalidResourceException(
                    "Too many palette colors.");

            if (_paletteColors > 0)
                ReadPalette();
            else if (_bitsPerPixel == 8 || _bitsPerPixel == 16 || _bitsPerPixel == 24 || _bitsPerPixel == 32)
                Logger.Trace("The image doesn't use a palette.");
            else
                throw new InvalidResourceException(
                    "The image format is not supported. Please report the issue on https://bitbucket.org/valeronm/amazfitbiptools");
            return ReadImage();
        }

        private void ReadHeader()
        {
            Logger.Trace("Reading image header...");

            if (IsVerge)
            {
                _width = (ushort)_reader.ReadUInt32();
                _height = (ushort)_reader.ReadUInt32();
                //_rowLengthInBytes = (ushort)_reader.ReadUInt32();

                _bitsPerPixel = (ushort)_reader.ReadUInt32();
                _paletteColors = (ushort)_reader.ReadUInt32();
                _transparency = _reader.ReadUInt32() > 0;

                _rowLengthInBytes = (ushort)Math.Ceiling(_width * _bitsPerPixel / 8.0);
                _paletteColors = 0;
            }
            else
            {
                _width = _reader.ReadUInt16();
                _height = _reader.ReadUInt16();
                _rowLengthInBytes = _reader.ReadUInt16();
                _bitsPerPixel = _reader.ReadUInt16();
                _paletteColors = _reader.ReadUInt16();
                _transparency = _reader.ReadUInt16() > 0;
            }

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
            if (_paletteColors > 0) return ReadPaletteImage();
            if (_bitsPerPixel == 8) return Read8BitImage();
            if (_bitsPerPixel == 16) return Read16BitImage();
            if (_bitsPerPixel == 24) return Read24BitImage();
            if (_bitsPerPixel == 32) return Read32BitImage();
            throw new InvalidResourceException($"Unsupported bits per pixel value: {_bitsPerPixel}");
        }

        private Bitmap ReadPaletteImage()
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
                        var pixelColorIndex = bitReader.ReadBits(_bitsPerPixel);
                        var color = _palette[(int)pixelColorIndex];
                        context.SetPixel(x, y, color);
                    }
                }
            }

            return image;
        }

        private Bitmap Read8BitImage()
        {
            var image = new Bitmap(_width, _height);
            using (var context = image.CreateUnsafeContext())
            {
                for (var y = 0; y < _height; y++)
                {
                    var rowBytes = _reader.ReadBytes(_rowLengthInBytes);
                    for (var x = 0; x < _width; x++)
                    {
                        var data = rowBytes[x];
                        var color = Color.FromArgb(0xff, data, data, data);
                        context.SetPixel(x, y, color);
                    }
                }
            }

            return image;
        }

        private Bitmap Read16BitImage()
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
                        var firstByte = (int)bitReader.ReadByte();
                        var secondByte = (int)bitReader.ReadByte();
                        var b = (byte)((secondByte >> 3) & 0x1f) << 3;
                        var g = (byte)(((firstByte >> 5) & 0x7) | ((secondByte & 0x07) << 3)) << 2;
                        var r = (byte)(firstByte & 0x1f) << 3;
                        var color = Color.FromArgb(0xff, r, g, b);
                        context.SetPixel(x, y, color);
                    }
                }
            }

            return image;
        }

        private Bitmap Read24BitImage()
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
                        var alpha = (int)bitReader.ReadByte();
                        var b = (int)(bitReader.ReadBits(5) << 3);
                        var g = (int)(bitReader.ReadBits(6) << 2);
                        var r = (int)(bitReader.ReadBits(5) << 3);
                        var color = Color.FromArgb(0xff - alpha, r, g, b);
                        context.SetPixel(x, y, color);
                    }
                }
            }

            return image;
        }

        private Bitmap Read32BitImage()
        {
            var image = new Bitmap(_width, _height);
            using (var context = image.CreateUnsafeContext())
            {
                for (var y = 0; y < _height; y++)
                {
                    var rowBytes = _reader.ReadBytes(_rowLengthInBytes);

                    for (var x = 0; x < _width; x++)
                    {
                        var r = rowBytes[x * 4];
                        var g = rowBytes[x * 4 + 1];
                        var b = rowBytes[x * 4 + 2];
                        var alpha = rowBytes[x * 4 + 3];
                        Logger.Trace("WriteColor A {0}: R {1}, G {2}, B {3}", alpha, r, g, b);
                        var color = Color.FromArgb(0xff - alpha, r, g, b);

                        if (IsVerge)
                        {
                            //if (IsInverted)
                                color = Color.FromArgb(alpha, b, g, r);
                            //else
                            //    color = Color.FromArgb(alpha, r, g, b);
                        }

                        context.SetPixel(x, y, color);
                    }
                }
            }

            return image;
        }
    }
}