using Newtonsoft.Json;
using WatchFace.Parser.JsonConverters;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.BasicElements
{
    public class CircleScale
    {
        [RawParameter(Id = 1)]
        public long CenterX { get; set; }

        [RawParameter(Id = 2)]
        public long CenterY { get; set; }

        [RawParameter(Id = 3)]
        public long RadiusX { get; set; }

        [RawParameter(Id = 4)]
        public long RadiusY { get; set; }

        [RawParameter(Id = 5)]
        public long StartAngle { get; set; }

        [RawParameter(Id = 6)]
        public long EndAngle { get; set; }

        [RawParameter(Id = 7)]
        public long Width { get; set; }

        [JsonConverter(typeof(HexStringJsonConverter))]
        [RawParameter(Id = 8)]
        public long Color { get; set; }
    }
}