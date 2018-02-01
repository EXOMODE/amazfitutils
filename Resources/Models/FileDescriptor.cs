using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

namespace Resources.Models
{
    public class FileDescriptor
    {
        public bool HasNewHeader { get; set; }
        public uint? ResourcesCount { get; set; }
        public uint? Unknown { get; set; }
        public byte? Version { get; set; }

        [IgnoreDataMember]
        public List<Bitmap> Images { get; set; } = new List<Bitmap>();
    }
}