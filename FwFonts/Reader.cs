using System.Collections.Generic;
using System.Drawing;
using System.IO;
using FwFonts.Models;
using NLog;

namespace FwFonts
{
    public class Reader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly BinaryReader _binaryReader;
        private readonly Stream _stream;

        public Reader(Stream stream)
        {
            _stream = stream;
            _binaryReader = new BinaryReader(_stream);
        }

        public Dictionary<char, Bitmap> ReadBlock(BlockDescriptor block)
        {
            _stream.Seek(block.GlyphsFileOffset, SeekOrigin.Begin);

            Logger.Trace("Reading block of symbols from '{0}' to '{1}' images...", block.StartSymbol, block.EndSymbol);
            var glyphs = new List<GlyphDescriptor>(block.EndSymbol - block.StartSymbol);
            for (var currentChar = block.StartSymbol; currentChar <= block.EndSymbol; currentChar++)
            {
                Logger.Trace("Reading descriptor of symbol '{0}'...", currentChar);
                glyphs.Add(GlyphDescriptor.ReadFrom(_binaryReader, currentChar));
            }

            var images = new Dictionary<char, Bitmap>(glyphs.Count);
            foreach (var glyphDescriptor in glyphs)
            {
                Logger.Debug("Reading image of symbol '{0}'...", glyphDescriptor.Symbol);
                images[glyphDescriptor.Symbol] = new GlyphReader(_stream).Read(glyphDescriptor);
            }

            return images;
        }
    }
}