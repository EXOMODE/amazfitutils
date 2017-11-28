using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements.StatusElements
{
    public class Switch
    {
        [ParameterId(1)]
        public Coordinates Coordinates { get; set; }

        [ParameterId(2)]
        [ParameterImageIndex]
        public long? ImageIndexOn { get; set; }

        [ParameterId(3)]
        [ParameterImageIndex]
        public long? ImageIndexOff { get; set; }
    }
}