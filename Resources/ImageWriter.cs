using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NLog;

namespace Resources
{
    public class ImageWriter
    {
        private const int MaxSupportedColors = 9;
        private static readonly byte[] Signature = {(byte) 'B', (byte) 'M', (byte) 'd', 0};
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Bitmap _image;
        private readonly List<Color> _palette;

        private readonly BinaryWriter _writer;

        private ushort _bitsPerPixel;
        private ushort _height;
        private ushort _paletteColors;
        private ushort _rowLengthInBytes;
        private ushort _transparency;
        private ushort _width;

        public ImageWriter(Stream stream, Bitmap image)
        {
            _writer = new BinaryWriter(stream);
            _image = image;
            _palette = new List<Color>();
        }

        public void Write()
        {
            _writer.Write(Signature);

            _width = (ushort) _image.Width;
            _height = (ushort) _image.Height;
            ExtractPalette();

            _paletteColors = (ushort) _palette.Count;
            _bitsPerPixel = (ushort) Math.Ceiling(Math.Log(_paletteColors, 2));
            if (_bitsPerPixel == 3)
                _bitsPerPixel = 4;

            if (_bitsPerPixel != 1 && _bitsPerPixel != 2 && _bitsPerPixel != 4)
                throw new ArgumentException($"{0} bits per pixel doesn't supported.");

            _rowLengthInBytes = (ushort) Math.Ceiling(_width * _bitsPerPixel / 8.0);

            if (_paletteColors > MaxSupportedColors)
                Logger.Warn($"Image cotains {0} colors but biggest known supported values is {1} colors",
                    _paletteColors, MaxSupportedColors);

            WriteHeader();
            WritePalette();
            WriteImage();
        }

        private void ExtractPalette()
        {
            Logger.Trace("Extracting palette...");
            for (var y = 0; y < _height; y++)
            for (var x = 0; x < _width; x++)
            {
                var color = _image.GetPixel(x, y);
                if (_palette.Contains(color)) continue;

                if (color.A == 0 && _transparency == 0)
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

            for (var y = 0; y < _height; y++)
            {
                var rowData = new byte[_rowLengthInBytes];
                var memoryStream = new MemoryStream(rowData);
                var bitWriter = new BitWriter(memoryStream);
                for (var x = 0; x < _width; x++)
                {
                    var color = _image.GetPixel(x, y);
                    var paletteIndex = paletteHash[color];
                    bitWriter.WriteBits(paletteIndex, _bitsPerPixel);
                }
                bitWriter.Flush();
                _writer.Write(rowData);
            }
        }
    }
}