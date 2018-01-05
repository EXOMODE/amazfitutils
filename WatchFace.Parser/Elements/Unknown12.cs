using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements
{
    public class Unknown12
    {
        [ParameterId(1)]
        public UnknownBlock Unknown1 { get; set; }

        [ParameterId(2)]
        public Image Unknown2 { get; set; }

        [ParameterId(3)]
        public Image Unknown3 { get; set; }

        [ParameterId(4)]
        public Image Unknown4 { get; set; }

        [ParameterId(5)]
        public Image Unknown5 { get; set; }
    }
}