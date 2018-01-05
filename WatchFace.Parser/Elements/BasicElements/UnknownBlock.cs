using WatchFace.Parser.Attributes;

namespace WatchFace.Parser.Elements.BasicElements
{
    public class UnknownBlock
    {
        [ParameterId(1)]
        public long TopLeftX { get; set; }

        [ParameterId(2)]
        public long TopLeftY { get; set; }

        [ParameterId(3)]
        public long BottomRightX { get; set; }

        [ParameterId(4)]
        public long BottomRightY { get; set; }
    }
}