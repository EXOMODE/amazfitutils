using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.ActivityElements;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements
{
    public class ActivityVerge : Activity
    {
        [ParameterId(1)]
        public new FormattedNumber Distance { get; set; }

        [ParameterId(3)]
        public new Number Pulse { get; set; }

        [ParameterId(5)]
        public new Steps Steps { get; set; }

        [ParameterId(9)]
        public Image CircleRange { get; set; }
    }
}