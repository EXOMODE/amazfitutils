using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.ActivityElements;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements
{
    public class ActivityBip : Activity
    {
        [ParameterId(1)]
        public new Number Steps { get; set; }

        [ParameterId(2)]
        public new Number StepsGoal { get; set; }

        [ParameterId(3)]
        public new Number Calories { get; set; }

        [ParameterId(4)]
        public new Number Pulse { get; set; }

        [ParameterId(5)]
        public new FormattedNumber Distance { get; set; }
    }
}