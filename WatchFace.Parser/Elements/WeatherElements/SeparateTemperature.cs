using Newtonsoft.Json;
using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements.WeatherElements
{
    public class SeparateTemperature
    {
        [ParameterId(1)]
        public TemperatureNumber Day { get; set; }

        [ParameterId(2)]
        public TemperatureNumber Night { get; set; }

        [ParameterId(3)]
        public Coordinates DayAlt { get; set; }

        [ParameterId(4)]
        public Coordinates NightAlt { get; set; }

        // For compatibility with "Unknown3" JSON attribute
        [JsonProperty("Unknown3")]
        private Coordinates Unknown3
        {
            set { DayAlt = value; }
        }

        // For compatibility with "Unknown4" JSON attribute
        [JsonProperty("Unknown4")]
        private Coordinates Unknown4
        {
            set { NightAlt = value; }
        }
    }
}