using WatchFace.Elements.BasicElements;
using WatchFace.Utils;

namespace WatchFace.Elements.WeatherElements
{
    public class SeparateTemperature
    {
        [RawParameter(Id = 1)]
        public TemperatureNumber Day { get; set; }

        [RawParameter(Id = 2)]
        public TemperatureNumber Night { get; set; }

        [RawParameter(Id = 3)]
        public Coordinates Unknown3 { get; set; }

        [RawParameter(Id = 4)]
        public Coordinates Unknown4 { get; set; }
    }
}