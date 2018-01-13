using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements
{
    public class AnalogDialBitmaps
    {
        [ParameterId(1)]
        public Rectangle RedrawBlock { get; set; }

        [ParameterId(2)]
        public Image Hours { get; set; }

        [ParameterId(3)]
        public Image Minutes { get; set; }

        [ParameterId(4)]
        public Image Seconds { get; set; }

        [ParameterId(5)]
        public Image CenterImage { get; set; }
    }
}