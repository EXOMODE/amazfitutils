using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.WeatherElements
{
    public class TodayTemperature
    {
        [RawParameter(Id = 1)]
        public SeparateTemperature Separate { get; set; }

        [RawParameter(Id = 2)]
        public OneLineTemperature OneLine { get; set; }
    }
}