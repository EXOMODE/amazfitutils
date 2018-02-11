using System.IO;

namespace FwFonts.Models
{
    public class BlockDescriptor
    {
        public char StartSymbol { get; set; }
        public char EndSymbol { get; set; }
        public uint GlyphsOffset { get; set; }
        public uint NextBlockOffset { get; set; }

        public bool HasNextBlock => NextBlockOffset > 0;
        public uint NextBlockFileOffset => NextBlockOffset - Constants.FirmwareBase;
        public uint GlyphsFileOffset => GlyphsOffset - Constants.FirmwareBase;

        public static BlockDescriptor ReadFrom(BinaryReader reader)
        {
            return new BlockDescriptor
            {
                StartSymbol = (char) reader.ReadUInt16(),
                EndSymbol = (char) reader.ReadUInt16(),
                GlyphsOffset = reader.ReadUInt32(),
                NextBlockOffset = reader.ReadUInt32()
            };
        }
    }
}