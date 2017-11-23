using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Elements.ActivityElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements
{
    public class Activity
    {
        [RawParameter(Id = 3)]
        public Number Calories { get; set; }

        [RawParameter(Id = 4)]
        public Number Pulse { get; set; }

        [RawParameter(Id = 1)]
        public Number Steps { get; set; }

        [RawParameter(Id = 2)]
        public Number StepsGoal { get; set; }

        [RawParameter(Id = 5)]
        public FormattedNumber Distance { get; set; }
    }
}