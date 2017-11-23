using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.TimeElements
{
    public class AmPm
    {
        [RawParameter(Id = 4)]
        public long ImageIndexPm { get; set; }

        [RawParameter(Id = 3)]
        public long ImageIndexAm { get; set; }

        [RawParameter(Id = 1)]
        public long X { get; set; }

        [RawParameter(Id = 2)]
        public long Y { get; set; }
    }
}