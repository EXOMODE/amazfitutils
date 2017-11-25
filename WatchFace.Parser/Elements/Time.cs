using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.TimeElements;

namespace WatchFace.Parser.Elements
{
    public class Time
    {
        [ParameterId(1)]
        public TwoDigits Hours { get; set; }

        [ParameterId(2)]
        public TwoDigits Minutes { get; set; }

        [ParameterId(3)]
        public TwoDigits Seconds { get; set; }

        [ParameterId(4)]
        public AmPm AmPm { get; set; }
    }
}