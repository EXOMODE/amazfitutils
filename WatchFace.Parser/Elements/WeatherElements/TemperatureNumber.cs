using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.WeatherElements
{
    public class TemperatureNumber
    {
        [RawParameter(Id = 1)]
        public Number Number { get; set; }

        [RawParameter(Id = 2)]
        public long MinusImageIndex { get; set; }

        [RawParameter(Id = 3)]
        public long DegreesImageIndex { get; set; }
    }
}