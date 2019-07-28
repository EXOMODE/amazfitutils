using WatchFace.Parser.Attributes;

namespace WatchFace.Parser.Elements.BasicElements
{
    public class Number3
    {
        [ParameterId(1)]
        public long TopLeftX { get; set; }

        [ParameterId(2)]
        public long TopLeftY { get; set; }

        [ParameterId(3)]
        public long BottomRightX { get; set; }
    }
}