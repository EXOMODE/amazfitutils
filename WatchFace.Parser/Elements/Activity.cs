using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.ActivityElements;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements
{
    public class Activity
    {
        [ParameterId(1)]
        public CompositeNumber Steps { get; set; }

        [ParameterId(2)]
        public CompositeNumber StepsGoal { get; set; }

        [ParameterId(3)]
        public CompositeNumber Calories { get; set; }

        [ParameterId(4)]
        public CompositeNumber Pulse { get; set; }

        [ParameterId(5)]
        public DistanceNumber Distance { get; set; }

        [ParameterId(6)]
        public StepsWithGoalNumber StepsWithGoal { get; set; }
    }
}