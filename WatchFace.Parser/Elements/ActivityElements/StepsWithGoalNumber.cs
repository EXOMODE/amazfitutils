using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements.ActivityElements
{
    public class StepsWithGoalNumber
    {
        [ParameterId(1)]
        public Number Number { get; set; }

        [ParameterId(2)]
        [ParameterImageIndex]
        public long? DelimiterImageIndex { get; set; }
    }
}