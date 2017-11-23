using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using WatchFace.Utils;

namespace WatchFace
{
    public class ImageReader
    {
        private readonly BinaryReader _reader;
        private ushort _bitsPerPixel;
        private ushort _height;
        private List<Color> _palette;
        private ushort _paletteColors;
        private ushort _rowLengthInBytes;
        private ushort _transp;
        private ushort _width;

        public ImageReader(Stream stream)
        {
            _reader = new BinaryReader(stream);
        }

        public Bitmap Read()
        {
            var signature = _reader.ReadChars(4);
            if (signature[0] != 'B' || signature[1] != 'M')
                throw new ArgumentOutOfRangeException("signature", "Signature doesn't match.");

            ReadHeader();
            ReadPalette();
            return ReadImage();
        }

        private void ReadHeader()
        {
            _width = _reader.ReadUInt16();
            _height = _reader.ReadUInt16();
            _rowLengthInBytes = _reader.ReadUInt16();
            _bitsPerPixel = _reader.ReadUInt16();
            _paletteColors = _reader.ReadUInt16();
            _transp = _reader.ReadUInt16();
        }

        private void ReadPalette()
        {
            _palette = new List<Color>(_paletteColors);
            for (var i = 0; i < _paletteColors; i++)
            {
                var r = _reader.ReadByte();
                var g = _reader.ReadByte();
                var b = _reader.ReadByte();
                var a = _reader.ReadByte();

                _palette.Add(Color.FromArgb(a, r, g, b));
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