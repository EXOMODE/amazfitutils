using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements.WeatherElements
{
    public class OneLineTemperature
    {
        [ParameterId(1)]
        public Number Number { get; set; }

        [ParameterId(2)]
        [ParameterImageIndex]
        public long MinusSignImageIndex { get; set; }

        [ParameterId(3)]
        [ParameterImageIndex]
        public long DelimiterImageIndex { get; set; }

        [ParameterId(4)]
        public bool AppendDegreesForBoth { get; set; }

        [ParameterId(5)]
        [ParameterImageIndex]
        public long DegreesImageIndex { get; set; }
    }
}