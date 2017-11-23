using WatchFace.Elements.BasicElements;
using WatchFace.Utils;

namespace WatchFace.Elements.WeatherElements
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
        public long Unknown4 { get; set; }

        [RawParameter(Id = 5)]
        public long DegreesImageIndex { get; set; }
    }
}