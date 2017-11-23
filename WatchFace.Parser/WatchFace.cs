using WatchFace.Elements;
using WatchFace.Utils;

namespace WatchFace
{
    public class WatchFace
    {
        [RawParameter(Id = 2)]
        public Background Background { get; set; }

        [RawParameter(Id = 3)]
        public Time Time { get; set; }

        [RawParameter(Id = 4)]
        public Activity Activity { get; set; }

        [RawParameter(Id = 5)]
        public Date Date { get; set; }

        [RawParameter(Id = 6)]
        public Weather Weather { get; set; }

        [RawParameter(Id = 7)]
        public Scales Scales { get; set; }

        [RawParameter(Id = 8)]
        public Status Status { get; set; }

        [RawParameter(Id = 9)]
        public Battery Battery { get; set; }

        [RawParameter(Id = 10)]
        public AnalogDialFace AnalogDialFace { get; set; }
    }
}