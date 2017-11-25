using Newtonsoft.Json;
using WatchFace.Parser.JsonConverters;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.BasicElements
{
    public class CircleScale
    {
        [ParameterId(1)]
        public long CenterX { get; set; }

        [ParameterId(2)]
        public long CenterY { get; set; }

        [ParameterId(3)]
        public long RadiusX { get; set; }

        [ParameterId(4)]
        public long RadiusY { get; set; }

        [ParameterId(5)]
        public long StartAngle { get; set; }

        [ParameterId(6)]
        public long EndAngle { get; set; }

        [ParameterId(7)]
        public long Width { get; set; }

        [JsonConverter(typeof(HexStringJsonConverter))]
        [ParameterId(8)]
        public long Color { get; set; }
    }
}