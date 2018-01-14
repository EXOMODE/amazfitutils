using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements
{
    public class Battery
    {
        [ParameterId(1)]
        public CompositeNumber Text { get; set; }

        [ParameterId(2)]
        public ImageSet Icon { get; set; }

        [ParameterId(3)]
        public Scale Scale { get; set; }
    }
}