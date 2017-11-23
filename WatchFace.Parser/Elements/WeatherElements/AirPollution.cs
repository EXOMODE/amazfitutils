using WatchFace.Elements.BasicElements;
using WatchFace.Utils;

namespace WatchFace.Elements.WeatherElements
{
    public class AirPollution
    {
        [RawParameter(Id = 2)]
        public ImageSet Icon { get; set; }
    }
}