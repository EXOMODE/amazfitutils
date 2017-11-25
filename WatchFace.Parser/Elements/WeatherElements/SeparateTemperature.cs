using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.WeatherElements
{
    public class SeparateTemperature
    {
        [ParameterId(1)]
        public TemperatureNumber Day { get; set; }

        [ParameterId(2)]
        public TemperatureNumber Night { get; set; }

        [ParameterId(3)]
        public Coordinates Unknown3 { get; set; }

        [ParameterId(4)]
        public Coordinates Unknown4 { get; set; }
    }
}