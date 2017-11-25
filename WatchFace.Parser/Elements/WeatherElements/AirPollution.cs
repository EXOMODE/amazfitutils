using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.WeatherElements
{
    public class AirPollution
    {
        // TODO: Looks like here should be Id 1 also

        [ParameterId(2)]
        public ImageSet Icon { get; set; }
    }
}