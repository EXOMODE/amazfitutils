using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements.WeatherElements
{
    public class WeatherIcon
    {
        [ParameterId(1)]
        public Coordinates Coordinates { get; set; }

        // TODO: Looks like here should be Id 2 also

        [ParameterId(3)]
        public Coordinates Unknown3 { get; set; }

        [ParameterId(4)]
        public Coordinates Unknown4 { get; set; }
    }
}