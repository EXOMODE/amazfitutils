using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.ActivityElements;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements
{
    public class Activity
    {
        [ParameterId(1)]
        public Number Steps { get; set; }

        [ParameterId(2)]
        public Number StepsGoal { get; set; }

        [ParameterId(3)]
        public Number Calories { get; set; }

        [ParameterId(4)]
        public Number Pulse { get; set; }

        [ParameterId(5)]
        public FormattedNumber Distance { get; set; }
    }
}