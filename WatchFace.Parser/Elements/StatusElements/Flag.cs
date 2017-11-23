using WatchFace.Elements.BasicElements;
using WatchFace.Utils;

namespace WatchFace.Elements.StatusElements
{
    public class Flag
    {
        [RawParameter(Id = 1)]
        public Coordinates Coordinates { get; set; }

        [RawParameter(Id = 2)]
        public long ImageIndex { get; set; }
    }
}