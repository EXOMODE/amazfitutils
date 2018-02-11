using System.Collections.Generic;

namespace FwFonts.Models
{
    public class FirmweareDescriptor
    {
        public string Version { get; set; }
        public Dictionary<string, FontDescriptor> Fonts { get; set; } = new Dictionary<string, FontDescriptor>();
    }
}