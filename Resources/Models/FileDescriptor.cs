using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public List<IResource> Resources { get; set; } = new List<IResource>();
    }
}