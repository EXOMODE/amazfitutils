using WatchFace.Elements.BasicElements;
using WatchFace.Utils;

namespace WatchFace.Elements.WeatherElements
{
    public class WeatherIcon
    {
        [RawParameter(Id = 1)]
        public Coordinates Coordinates { get; set; }

        [RawParameter(Id = 3)]
        public Coordinates Unknown3 { get; set; }

        [RawParameter(Id = 4)]
        public Coordinates Unknown4 { get; set; }
    }
}