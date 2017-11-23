using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.WeatherElements
{
    public class Temperature
    {
        [RawParameter(Id = 1)]
        public TemperatureNumber Current { get; set; }

        [RawParameter(Id = 2)]
        public TodayTemperature Today { get; set; }
    }
}