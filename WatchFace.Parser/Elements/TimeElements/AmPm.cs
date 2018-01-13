using WatchFace.Parser.Attributes;

namespace WatchFace.Parser.Elements.TimeElements
{
    public class AmPm
    {
        [ParameterId(1)]
        public long X { get; set; }

        [ParameterId(2)]
        public long Y { get; set; }

        [ParameterId(3)]
        [ParameterImageIndex]
        public long ImageIndexAm { get; set; }

        [ParameterId(4)]
        public long PmX { get; set; }

        [ParameterId(5)]
        public long PmY { get; set; }

        [ParameterId(6)]
        [ParameterImageIndex]
        public long ImageIndexPm { get; set; }
    }
}