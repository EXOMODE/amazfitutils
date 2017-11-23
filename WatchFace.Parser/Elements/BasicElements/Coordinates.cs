using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.BasicElements
{
    public class Coordinates
    {
        [RawParameter(Id = 1)]
        public long X { get; set; }

        [RawParameter(Id = 2)]
        public long Y { get; set; }
    }
}