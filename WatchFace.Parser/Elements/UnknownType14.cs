using Newtonsoft.Json;
using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Elements.TimeElements;
using WatchFace.Parser.JsonConverters;

namespace WatchFace.Parser.Elements
{
    public class UnknownType14
    {
        [ParameterId(1)]
        public TwoDigits Unknown1 { get; set; }

        [ParameterId(2)]
        public TwoDigits Unknown2 { get; set; }

        [ParameterId(6)]
        public UnknownType14d6 Unknown6 { get; set; }

        [ParameterId(7)]
        public UnknownType14d6 Unknown7 { get; set; }

        [ParameterId(8)]
        public UnknownType14d6 Unknown8 { get; set; }
    }
}