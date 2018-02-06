using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

namespace Fonts.Models
{
    public class FileDescriptor
    {
        public bool HasNewHeader { get; set; }
        public uint? BlocksCount { get; set; }
        public uint? Unknown { get; set; }
        public byte? Version { get; set; }

        [IgnoreDataMember]
        public List<BlockDescriptor> Blocks { get; set; } = new List<BlockDescriptor>();

        [IgnoreDataMember]
        public Dictionary<char, Bitmap> Images { get; set; } = new Dictionary<char, Bitmap>();
    }
}