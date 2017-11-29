using System.Collections.Generic;
using System.Drawing;

namespace Resources.Models
{
    public class FileDescriptor
    {
        public byte? Version { get; set; }
        public List<Bitmap> Images { get; set; } = new List<Bitmap>();
    }
}