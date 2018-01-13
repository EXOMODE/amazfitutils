using WatchFace.Parser.Attributes;

namespace WatchFace.Parser.Elements.WeatherElements
{
    public class CustomWeatherIcon
    {
        [ParameterId(1)]
        public long X { get; set; }

        [ParameterId(2)]
        public long Y { get; set; }

        [ParameterId(3)]
        [ParameterImageIndex]
        public long ImageIndex { get; set; }

        [ParameterId(4)]
        [ParameterImagesCount]
        public long ImagesCount { get; set; }
    }
}