using WatchFace.Parser.Attributes;

namespace WatchFace.Parser.Elements.BasicElements
{
    public class XY
    {
        [ParameterId(1)]
        public long X { get; set; }

        [ParameterId(2)]
        public long Y { get; set; }
    }
}