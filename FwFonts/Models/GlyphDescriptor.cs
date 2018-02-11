using System.IO;

namespace FwFonts.Models
{
    public class GlyphDescriptor
    {
        public byte Width { get; set; }
        public byte Height { get; set; }
        public byte OffsetX { get; set; }
        public byte OffsetY { get; set; }
        public ushort DrawWidth { get; set; }
        public ushort Unknown6 { get; set; }
        public uint DataOffset { get; set; }

        public char Symbol { get; set; }

        public int RowLength => Width % 8 == 0 ? Width / 8 : Width / 8 + 1;
        public uint DataFileOffset => DataOffset - Constants.FirmwareBase;

        public static GlyphDescriptor ReadFrom(BinaryReader reader, char symbol)
        {
            return new GlyphDescriptor
            {
                Width = reader.ReadByte(),
                Height = reader.ReadByte(),
                OffsetX = reader.ReadByte(),
                OffsetY = reader.ReadByte(),
                DrawWidth = reader.ReadUInt16(),
                Unknown6 = reader.ReadUInt16(),
                DataOffset = reader.ReadUInt32(),
                Symbol = symbol
            };
        }
    }
}