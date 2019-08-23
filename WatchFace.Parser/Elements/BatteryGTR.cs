using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements
{
    public class BatteryGTR
    {
        [ParameterId(1)]
        public Number Text { get; set; }

        [ParameterId(2)]
        public ImageSet Images { get; set; }

        [ParameterId(3)]
        public ImageSet Icon { get; set; }

        [ParameterId(6)]
        public Image Persent { get; set; }

        [ParameterId(7)]
        public CircleScale Scale { get; set; }
    }
}