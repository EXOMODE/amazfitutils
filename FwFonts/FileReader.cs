using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using FwFonts.Models;
using NLog;

namespace FwFonts
{
    public class FileReader
    {
        private const uint VersionOffset = 0x803BC80;

        private static readonly Dictionary<string, uint> FontOffsets = new Dictionary<string, uint>
        {
            ["wf1"] = 0x806B848,
            ["wf5"] = 0x806BB70,
            ["font3"] = 0x806BF84,
            ["font2"] = 0x806C388,
            ["font1"] = 0x806C9F4,
            ["wf7_time"] = 0x806CFDC,
            ["wf8_time"] = 0x806D4EC,
            ["wf6"] = 0x806D674,
            ["font4"] = 0x806D7C4,
            ["wf7_wf8_small"] = 0x806DA90,
            ["wf8_big"] = 0x806DD2C
        };

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static FirmweareDescriptor Read(Stream stream)
        {
            var binaryReader = new BinaryReader(stream);

            stream.Seek(VersionOffset - Constants.FirmwareBase, SeekOrigin.Begin);

            var version = Encoding.ASCII.GetString(binaryReader.ReadBytes(8));
            Logger.Debug("Firmware version was read:");
            Logger.Debug("Version: {0}", version);

            var result = new FirmweareDescriptor {Fonts = new Dictionary<string, FontDescriptor>()};

            foreach (var fontOffset in FontOffsets)
            {
                var font = new FontDescriptor
                {
                    Images = new Dictionary<char, Bitmap>(),
                    Blocks = new List<BlockDescriptor>()
                };

                var offset = fontOffset.Value - Constants.FirmwareBase;
                var name = fontOffset.Key;
                Logger.Debug("Reading font '{0}' from offset 0x{1}", name, offset);
                BlockDescriptor block;
                do
                {
                    stream.Seek(offset, SeekOrigin.Begin);
                    block = BlockDescriptor.ReadFrom(binaryReader);
                    var blockImages = new Reader(stream).ReadBlock(block);

                    foreach (var blockImage in blockImages)
                        font.Images.Add(blockImage.Key, blockImage.Value);
                    font.Blocks.Add(block);

                    offset = block.NextBlockFileOffset;
                } while (block.NextBlockOffset > 0);

                result.Fonts[name] = font;
            }

            return result;
        }
    }
}