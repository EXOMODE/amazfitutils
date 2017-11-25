using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.WeatherElements
{
    public class TemperatureNumber
    {
        [ParameterId(1)]
        public Number Number { get; set; }

        [ParameterId(2)]
        [ParameterImageIndex]
        public long MinusImageIndex { get; set; }

        [ParameterId(3)]
        [ParameterImageIndex]
        public long DegreesImageIndex { get; set; }
    }
}