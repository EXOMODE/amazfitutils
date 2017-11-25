using WatchFace.Parser.Elements.WeatherElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements
{
    public class Weather
    {
        [ParameterId(1)]
        public WeatherIcon Icon { get; set; }

        [ParameterId(2)]
        public Temperature Temperature { get; set; }

        [ParameterId(3)]
        public AirPollution AirPollution { get; set; }
    }
}