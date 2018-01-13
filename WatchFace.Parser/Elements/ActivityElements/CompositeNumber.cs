using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements.ActivityElements
{
    public class CompositeNumber
    {
        [ParameterId(1)]
        public Number Number { get; set; }

        [ParameterId(2)]
        [ParameterImageIndex]
        public long? PrefixImageIndex { get; set; }

        [ParameterId(3)]
        [ParameterImageIndex]
        public long? EmptyImageIndex { get; set; }

        [ParameterId(4)]
        [ParameterImageIndex]
        public long? SuffixImageIndex { get; set; }
    }
}