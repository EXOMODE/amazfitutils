using System.Collections.Generic;
using Newtonsoft.Json;
using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.JsonConverters;

namespace WatchFace.Parser.Elements.AnalogDialFaceElements
{
    public class ClockHandVerge
    {
        [ParameterId(1)]
        public long? unknown1 { get; set; }

        [ParameterId(2)]
        public long? unknown2 { get; set; }

        [ParameterId(3)]
        public XY unknown3 { get; set; }

        [ParameterId(4)]
        public XY unknown4 { get; set; }

        [ParameterId(5)]
        public Image CenterImage { get; set; }
    }
}