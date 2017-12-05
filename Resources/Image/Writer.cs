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
        private static readonly byte[] Signature = {(byte) 'B', (byte) 'M', (byte) 'd', 0};
        private readonly List<Color> _palette;

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

        public void Write(Bitmap image)
        {
            _image = image;
            _width = (ushort) image.Width;
            _height = (ushort) image.Height;

            ApplyDithering();
            ExtractPalette();

            if (_bitsPerPixel == 3) _bitsPerPixel = 4;
            if (_bitsPerPixel == 0) _bitsPerPixel = 1;

            if (_bitsPerPixel > 4)
                throw new ArgumentException(
                    $"The image has {_bitsPerPixel} bit/pixel and can't be packed used on the watches. Looks like dithering works wincorrectly on the image."
                );

            _rowLengthInBytes = (ushort) Math.Ceiling(_width * _bitsPerPixel / 8.0);

            _writer.Write(Signature);

            WriteHeader();
            WritePalette();
            WriteImage();
        }

        private void ApplyDithering()
        {
            var clone = new Bitmap(_image.Width, _image.Height, PixelFormat.Format32bppArgb);
            using (var gr = Graphics.FromImage(clone))
            {
                gr.DrawImage(_image, new Rectangle(0, 0, clone.Width, clone.Height));
            }
            FloydSteinbergDitherer.Process(clone);
            _image = clone;
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

                    if (color.A < 0x80 && _transparency == 0)
                    {
                        Logger.Trace("Palette item {0}: R {1:X2}, G {2:X2}, B {3:X2}, Transaparent color",
                            _palette.Count, color.R, color.G, color.B
                        );
                        _palette.Insert(0, color);
                        _transparency = 1;
                    }
                    else
                    {
                        Logger.Trace("Palette item {0}: R {1:X2}, G {2:X2}, B {3:X2}",
                            _palette.Count, color.R, color.G, color.B
                        );
                        _palette.Add(color);
                    }
                }
            }
            _paletteColors = (ushort) _palette.Count;
            _bitsPerPixel = (ushort) Math.Ceiling(Math.Log(_paletteColors, 2));
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

        private void WriteImage()
        {
            Logger.Trace("Writing image...");

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
    }
}