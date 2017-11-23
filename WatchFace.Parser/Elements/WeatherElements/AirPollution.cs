using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.WeatherElements
{
    public class AirPollution
    {
        [RawParameter(Id = 2)]
        public ImageSet Icon { get; set; }
    }
}