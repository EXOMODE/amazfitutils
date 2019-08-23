using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.ActivityElements;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements
{
    public class ActivityGTR : Activity
    {
        [ParameterId(3)]
        public new Number Pulse { get; set; }

        [ParameterId(4)]
        public new FormattedNumber Distance { get; set; }

        [ParameterId(5)]
        public new Steps Steps { get; set; }

        [ParameterId(7)]
        public Image StarImage { get; set; }

        [ParameterId(9)]
        public Image CircleRange { get; set; }
    }
}