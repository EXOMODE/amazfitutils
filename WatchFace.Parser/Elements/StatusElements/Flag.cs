using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

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