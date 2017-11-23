using WatchFace.Parser.Elements.WeatherElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements
{
    public class Weather
    {
        [RawParameter(Id = 1)]
        public WeatherIcon Icon { get; set; }

        [RawParameter(Id = 2)]
        public Temperature Temperature { get; set; }

        [RawParameter(Id = 3)]
        public AirPollution AirPollution { get; set; }
    }
}