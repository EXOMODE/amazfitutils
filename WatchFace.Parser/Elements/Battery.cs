using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Elements.BatteryElements;

namespace WatchFace.Parser.Elements
{
    public class Battery
    {
        [ParameterId(1)]
        public BatteryNumber Text { get; set; }

        [ParameterId(2)]
        public ImageSet Icon { get; set; }

        [ParameterId(3)]
        public Scale Scale { get; set; }
    }
}