using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.ActivityElements;
using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Elements.DateElements;

namespace WatchFace.Parser.Elements
{
    public class Activity
    {
        [ParameterId(1)]
        public Steps Steps { get; set; }

        [ParameterId(2)]
        public Number StepsGoal { get; set; }

        [ParameterId(3)]
        public OneLineMonthAndDay Calories { get; set; }

        [ParameterId(4)]
        public PulseWrapper Pulse { get; set; }

        [ParameterId(5)]
        public FormattedNumber Distance { get; set; }
    }
}