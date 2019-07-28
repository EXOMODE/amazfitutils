using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.StatusElements;

namespace WatchFace.Parser.Elements
{
    public class Status
    {
        [ParameterId(1)]
        public Switch DoNotDisturb { get; set; }

        [ParameterId(2)]
        public Switch Lock { get; set; }

        [ParameterId(3)]
        public Switch Bluetooth { get; set; }

        [ParameterId(4)]
        public Battery Battery { get; set; }
    }
}