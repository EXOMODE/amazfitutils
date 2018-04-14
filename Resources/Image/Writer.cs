using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using BumpKit;
using NLog;
using Resources.Utils;

namespace Resources.Image
{
    public class Writer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly List<Color> _palette;
        private bool hasTransparentColor;
        private bool hasSemiTransparentColors;
        private bool hasPresiceColors;
        private ImageType imageType;

        private readonly BinaryWriter _writer;

        private ushort _bitsPerPixel;
        private ushort _height;
        private Bitmap _image;
        private ushort _paletteColors;
        private ushort _rowLengthInBytes;
        private ushort _transparency;
        private ushort _width;

        public Writer(Stream stream)
        {
            _writer = new BinaryWriter(stream);
            _palette = new List<Color>();
        }

        private byte[] Signature => new byte[] { (byte)'B', (byte)'M', (byte)imageType, 0 };

        public void Write(Bitmap image)
        {
            _image = image;
            _width = (ushort)image.Width;
            _height = (ushort)image.Height;

            ExtractPalette();

            if (hasSemiTransparentColors)
            {
                if (hasPresiceColors)
                {
                    imageType = ImageType.Bpp32;
                    _bitsPerPixel = 32;
                }
                else
                {
                    imageType = ImageType.Bpp24;
                    _bitsPerPixel = 24;
                }
            }
            else if (hasTransparentColor)
            {
                if (_bitsPerPixel > 8)
                {
                    imageType = ImageType.Bpp24;
                    _bitsPerPixel = 24;
                }
                else
                {
                    imageType = ImageType.Paletted;
                }
            } else
            {
                if (_bitsPerPixel > 8)
                {
                    imageType = ImageType.Bpp16;
                    _bitsPerPixel = 16;
                }
                else
                {
                    imageType = ImageType.Paletted;
                }
            }

            if (imageType == ImageType.Paletted) { 
                _paletteColors = (ushort)_palette.Count;
                if (hasTransparentColor) _transparency = 1;
                if (_bitsPerPixel > 4 && _bitsPerPixel <8) _bitsPerPixel = 8;
                if (_bitsPerPixel == 3) _bitsPerPixel = 4;
                if (_bitsPerPixel == 0) _bitsPerPixel = 1;
            }

            _rowLengthInBytes = (ushort)Math.Ceiling(_width * _bitsPerPixel / 8.0);

            _writer.Write(Signature);

            WriteHeader();

            switch (imageType)
            {
                case ImageType.Paletted:
                    WritePalette();
                    WritePalettedImage();
                    break;
                case ImageType.Bpp16:
                    Write16BitImage();
                    break;
                case ImageType.Bpp24:
                    Write24BitImage();
                    break;
                case ImageType.Bpp32:
                    Write32BitImage();
                    break;
            }
        }

        private void ExtractPalette()
        {
            Logger.Trace("Extracting palette...");

            using (var context = _image.CreateUnsafeContext())
            {
                for (var y = 0; y < _height; y++)
                for (var x = 0; x < _width; x++)
                {
                    var color = context.GetPixel(x, y);
                    if (_palette.Contains(color)) continue;

                    Logger.Trace("Palette item {0}: R {1:X2}, G {2:X2}, B {3:X2}, A {4:X2}",
                        _palette.Count, color.R, color.G, color.B, color.A
                    );

                    if (!hasTransparentColor && color.A == 0)
                    {
                        Logger.Trace("Found fully transaparent color");
                        _palette.Insert(0, color);
                        hasTransparentColor = true;
                    }
                    else
                    {
                        _palette.Add(color);
                    }

                    if (!hasSemiTransparentColors && color.A > 0 && color.A < 0xff)
                    {
                        Logger.Trace("Found semi transaparent color");
                        hasSemiTransparentColors = true;
                    }

                    if (!hasPresiceColors && ((color.R & 0x7) > 0 || (color.G & 0x3) > 0 || (color.B & 0x3) > 0))
                    {
                        Logger.Trace("Found presice color");
                            hasPresiceColors = true;
                    }
                }
            }
            _bitsPerPixel = (ushort)Math.Ceiling(Math.Log(_palette.Count, 2));
        }

        private void WriteHeader()
        {
            Logger.Trace("Writing image header...");
            Logger.Trace("Width: {0}, Height: {1}, RowLength: {2}", _width, _height, _rowLengthInBytes);
            Logger.Trace("BPP: {0}, PaletteColors: {1}, Transaparency: {2}",
                _bitsPerPixel, _paletteColors, _transparency
            );

            _writer.Write(_width);
            _writer.Write(_height);
            _writer.Write(_rowLengthInBytes);
            _writer.Write(_bitsPerPixel);
            _writer.Write(_paletteColors);
            _writer.Write(_transparency);
        }

        private void WritePalette()
        {
            Logger.Trace("Writing palette...");
            foreach (var color in _palette)
            {
                _writer.Write(color.R);
                _writer.Write(color.G);
                _writer.Write(color.B);
                _writer.Write((byte) 0); // always 0 maybe padding
            }
        }

        private void WritePalettedImage()
        {
            Logger.Trace("Writing paletted image...");

            var paletteHash = new Dictionary<Color, byte>();
            byte i = 0;
            foreach (var color in _palette)
            {
                paletteHash[color] = i;
                i++;
            }

            using (var context = _image.CreateUnsafeContext())
            {
                for (var y = 0; y < _height; y++)
                {
                    var rowData = new byte[_rowLengthInBytes];
                    var memoryStream = new MemoryStream(rowData);
                    var bitWriter = new BitWriter(memoryStream);
                    for (var x = 0; x < _width; x++)
                    {
                        var color = context.GetPixel(x, y);
                        if (color.A < 0x80 && _transparency == 1)
                        {
                            bitWriter.WriteBits(0, _bitsPerPixel);
                        }
                        else
                        {
                            var paletteIndex = paletteHash[color];
                            bitWriter.WriteBits(paletteIndex, _bitsPerPixel);
                        }
                    }

                    bitWriter.Flush();
                    _writer.Write(rowData);
                }
            }
        }

        private void Write16BitImage()
        {
            Logger.Trace("Writing 16-bit image...");
            using (var context = _image.CreateUnsafeContext())
            {
                for (var y = 0; y < _height; y++)
                {
                    var rowData = new byte[_rowLengthInBytes];
                    var memoryStream = new MemoryStream(rowData);
                    for (var x = 0; x < _width; x++)
                    {
                        var color = context.GetPixel(x, y);

                        byte first = (byte)((color.R >> 3) |((((color.G >> 2) & 0x7)) << 5));
                        byte second = (byte)(color.B | ((color.G >> 5) & 0x7));

                        memoryStream.WriteByte(first);
                        memoryStream.WriteByte(second);
                    }
                    _writer.Write(rowData);
                }
            }
        }

        private void Write24BitImage()
        {
            Logger.Trace("Writing 24-bit image...");
            using (var context = _image.CreateUnsafeContext())
            {
                for (var y = 0; y < _height; y++)
                {
                    var rowData = new byte[_rowLengthInBytes];
                    var memoryStream = new MemoryStream(rowData);
                    var bitWriter = new BitWriter(memoryStream);
                    for (var x = 0; x < _width; x++)
                    {
                        var color = context.GetPixel(x, y);

                        bitWriter.Write((byte)(0xff - color.A));
                        bitWriter.WriteBits((ulong)color.B >> 3, 5);
                        bitWriter.WriteBits((ulong)color.G >> 2, 6);
                        bitWriter.WriteBits((ulong)color.R >> 3, 5);
                    }
                    bitWriter.Flush();
                    _writer.Write(rowData);
                }
            }
        }

        private void Write32BitImage()
        {
            Logger.Trace("Writing 32-bit image...");
            using (var context = _image.CreateUnsafeContext())
            {
                for (var y = 0; y < _height; y++)
                {
                    var rowData = new byte[_rowLengthInBytes];
                    var memoryStream = new MemoryStream(rowData);
                    for (var x = 0; x < _width; x++)
                    {
                        var color = context.GetPixel(x, y);

                        memoryStream.WriteByte(color.R);
                        memoryStream.WriteByte(color.G);
                        memoryStream.WriteByte(color.B);
                        memoryStream.WriteByte((byte)(0xff - color.A));
                    }
                    _writer.Write(rowData);
                }
            }
        }
    }
}