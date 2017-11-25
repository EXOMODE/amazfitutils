using WatchFace.Parser.Attributes;

namespace WatchFace.Parser.Elements.WeatherElements
{
    public class Temperature
    {
        [ParameterId(1)]
        public TemperatureNumber Current { get; set; }

        [ParameterId(2)]
        public TodayTemperature Today { get; set; }
    }
}