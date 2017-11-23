using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.StatusElements
{
    public class Switch
    {
        [RawParameter(Id = 1)]
        public Coordinates Coordinates { get; set; }

        [RawParameter(Id = 2)]
        public long ImageIndexOn { get; set; }

        [RawParameter(Id = 3)]
        public long ImageIndexOff { get; set; }
    }
}