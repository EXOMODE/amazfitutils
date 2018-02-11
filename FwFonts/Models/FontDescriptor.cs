using System.Collections.Generic;
using System.Drawing;

namespace FwFonts.Models
{
    public class FontDescriptor
    {
        public List<BlockDescriptor> Blocks { get; set; } = new List<BlockDescriptor>();
        public Dictionary<char, Bitmap> Images { get; set; } = new Dictionary<char, Bitmap>();
    }
}