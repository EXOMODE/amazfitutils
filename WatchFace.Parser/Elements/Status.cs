using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.StatusElements;

namespace WatchFace.Parser.Elements
{
    public class Status
    {
        [ParameterId(1)]
        public Switch Bluetooth { get; set; }

        [ParameterId(2)]
        public Flag Alarm { get; set; }

        [ParameterId(3)]
        public Flag Lock { get; set; }

        [ParameterId(4)]
        public Flag DoNotDisturb { get; set; }
    }
}