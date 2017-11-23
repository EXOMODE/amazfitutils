using WatchFace.Elements.BasicElements;
using WatchFace.Elements.TimeElements;
using WatchFace.Utils;

namespace WatchFace.Elements
{
    public class Time
    {
        [RawParameter(Id = 1)]
        public TwoDigits Hours { get; set; }

        [RawParameter(Id = 2)]
        public TwoDigits Minutes { get; set; }

        [RawParameter(Id = 3)]
        public TwoDigits Seconds { get; set; }

        [RawParameter(Id = 4)]
        public AmPm AmPm { get; set; }
    }
}