using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.WeatherElements
{
    public class OneLineTemperature
    {
        [RawParameter(Id = 1)]
        public Number Number { get; set; }

        [RawParameter(Id = 2)]
        public long MinusSignImageIndex { get; set; }

        [RawParameter(Id = 3)]
        public long DelimiterImageIndex { get; set; }

        [RawParameter(Id = 4)]
        public long AppendDegreesForBoth { get; set; }

        [RawParameter(Id = 5)]
        public long DegreesImageIndex { get; set; }
    }
}