using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.StatusElements
{
    public class Flag
    {
        [RawParameter(Id = 1)]
        public Coordinates Coordinates { get; set; }

        [RawParameter(Id = 2)]
        public long ImageIndex { get; set; }
    }
}