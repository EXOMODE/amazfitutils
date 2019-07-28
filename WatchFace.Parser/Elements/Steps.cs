using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements
{
    public class Steps
    {
        [ParameterId(1)]
        public Number Step { get; set; }

        [ParameterId(2)]
        [ParameterImageIndex]
        public long? IconIndex { get; set; }
    }
}