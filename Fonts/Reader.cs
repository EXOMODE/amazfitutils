using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Fonts.Models;
using NLog;

namespace Fonts
{
    public class Reader
    {
        private const int BlockDescriptorLength = 6;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly BinaryReader _binaryReader;
        private readonly Stream _stream;

        public Reader(Stream stream)
        {
            _stream = stream;
            _binaryReader = new BinaryReader(_stream);
        }

        public List<BlockDescriptor> Read(uint blocksCount)
        {
            var descriptorsTableLength = (int) (blocksCount * BlockDescriptorLength);
            Logger.Trace("Reading font descriptors table with {0} elements ({1} bytes)",
                blocksCount, descriptorsTableLength
            );
            var descriptors = _binaryReader.ReadBytes(descriptorsTableLength);
            var blocks = new List<BlockDescriptor>();
            for (var i = 0; i < blocksCount; i++)
            {
                blocks.Add(new BlockDescriptor
                {
                    StartSymbol = BitConverter.ToChar(descriptors, i * BlockDescriptorLength),
                    EndSymbol = BitConverter.ToChar(descriptors, i * BlockDescriptorLength + 2),
                    Offset = BitConverter.ToUInt16(descriptors, i * BlockDescriptorLength + 4)
                });
            }

            return blocks;
        }

        public Dictionary<char, Bitmap> Read(List<BlockDescriptor> blocks)
        {
            var imagesOffset = _stream.Position;
            Logger.Debug("Reading {0} images...", blocks.Count);
            var images = new Dictionary<char, Bitmap>();
            for (var i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                var blockOffset = block.Offset;
                var realOffset = blockOffset + imagesOffset;
                Logger.Trace("Glyphs block {0} offset is {1}...", i, blockOffset);
                if (_stream.Position != realOffset)
                {
                    var bytesGap = realOffset - _stream.Position;
                    Logger.Warn("Found {0} bytes gap before block number {1}", bytesGap, i);
                    _stream.Seek(realOffset, SeekOrigin.Begin);
                }
                Logger.Debug("Reading block {0}...", i);

                for (var ch = block.StartSymbol; ch <= block.EndSymbol; ch++)
                {
                    images[ch] = new GlyphReader(_stream).Read(16, 16);
//                    images[ch].Save($"1\\{(int)ch:X4}.png", ImageFormat.Png);
                    var crc = _stream.ReadByte();
                }
            }
            return images;
        }
    }
}