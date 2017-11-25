using System.Collections.Generic;
using Newtonsoft.Json;
using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.JsonConverters;

namespace WatchFace.Parser.Elements.AnalogDialFaceElements
{
    public class ClockHand
    {
        [ParameterId(1)]
        public long Unknown1 { get; set; }

        [JsonConverter(typeof(HexStringJsonConverter))]
        [ParameterId(2)]
        public long Color { get; set; }

        [ParameterId(3)]
        public Coordinates Center { get; set; }

        [ParameterId(4)]
        public List<Coordinates> Shape { get; set; }

        [ParameterId(5)]
        public Image CenterImage { get; set; }
    }
}