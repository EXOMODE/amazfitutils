using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements.WeatherElements
{
    public class AirPollution
    {
        [ParameterId(1)]
        public Number Index { get; set; }

        [ParameterId(2)]
        public ImageSet Icon { get; set; }
    }
}