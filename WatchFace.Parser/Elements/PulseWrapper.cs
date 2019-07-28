using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements
{
    public class PulseWrapper
    {
        [ParameterId(1)]
        public Number Pulse { get; set; }

        [ParameterId(2)]
        [ParameterImageIndex]
        public long DelimiterImageIndex { get; set; }
    }
}