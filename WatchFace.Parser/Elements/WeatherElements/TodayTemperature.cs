using WatchFace.Parser.Attributes;

namespace WatchFace.Parser.Elements.WeatherElements
{
    public class TodayTemperature
    {
        [ParameterId(1)]
        public SeparateTemperature Separate { get; set; }

        [ParameterId(2)]
        public OneLineTemperature OneLine { get; set; }
    }
}