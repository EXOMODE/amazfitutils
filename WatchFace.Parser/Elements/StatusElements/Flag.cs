using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.StatusElements
{
    public class Flag
    {
        [ParameterId(1)]
        public Coordinates Coordinates { get; set; }

        [ParameterId(2)]
        [ParameterImageIndex]
        public long ImageIndex { get; set; }
    }
}