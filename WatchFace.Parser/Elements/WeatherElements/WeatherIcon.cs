using System.Runtime.Serialization;
using Newtonsoft.Json;
using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements.WeatherElements
{
    public class WeatherIcon
    {
        [ParameterId(1)]
        public Coordinates Coordinates { get; set; }

        [ParameterId(2)]
        public CustomWeatherIcon CustomIcon { get; set; }

        [ParameterId(3)]
        public Coordinates CoordinatesAlt { get; set; }

        [ParameterId(4)]
        public Coordinates Unknown4 { get; set; }

        // For compatibility with "Unknown3" JSON attribute
        [JsonProperty("Unknown3")]
        private Coordinates Unknown3
        {
            set { CoordinatesAlt = value; }
        }
    }
}