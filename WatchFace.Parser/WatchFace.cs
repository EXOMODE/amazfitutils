using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements;
using WatchFace.Parser.Elements.Animations;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser
{
    public class WatchFace
    {
        [ParameterId(2)]
        public Background Background { get; set; }

        [ParameterId(3)]
        public Time Time { get; set; }

        [ParameterId(4)]
        public Activity Activity { get; set; }

        [ParameterId(5)]
        public Date Date { get; set; }

        [ParameterId(6)]
        public Weather Weather { get; set; }

        [ParameterId(7)]
        public StepsProgress StepsProgress { get; set; }

        [ParameterId(8)]
        public Status Status { get; set; }

        [ParameterId(9)]
        public Battery Battery { get; set; }

        [ParameterId(10)]
        public AnalogDialFace AnalogDialFace { get; set; }

        [ParameterId(11)]
        public Other Other { get; set; }

        [ParameterId(12)]
        public Heart Heart { get; set; }

        [ParameterId(14)]
        public UnknownType14 Unknown14 { get; set; }
    }

    public class WatchFaceVerge : WatchFace
    {
        [ParameterId(4)]
        public new ActivityVerge Activity { get; set; }
    }

    public class WatchFaceBip : WatchFace
    {
        [ParameterId(4)]
        public new ActivityBip Activity { get; set; }

        [ParameterId(8)]
        public new StatusBip Status { get; set; }

        [ParameterId(9)]
        public new BatteryBip Battery { get; set; }
    }
}