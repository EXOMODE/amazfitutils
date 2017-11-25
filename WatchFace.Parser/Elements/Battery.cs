using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements
{
    public class Battery
    {
        [ParameterId(1)]
        public Number Text { get; set; }

        [ParameterId(2)]
        public ImageSet Icon { get; set; }

        [ParameterId(3)]
        public Scale Scale { get; set; }
    }
}