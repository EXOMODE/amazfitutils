using System.Collections.Generic;
using Newtonsoft.Json;
using WatchFace.Parser.JsonConverters;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.BasicElements
{
    public class ClockHand
    {
        [RawParameter(Id = 1)]
        public long Unknown1 { get; set; }

        [JsonConverter(typeof(HexStringJsonConverter))]
        [RawParameter(Id = 2)]
        public long Color { get; set; }

        [RawParameter(Id = 3)]
        public Coordinates Center { get; set; }

        [RawParameter(Id = 4)]
        public List<Coordinates> Shape { get; set; }

        [RawParameter(Id = 5)]
        public Image CenterImage { get; set; }
    }
}