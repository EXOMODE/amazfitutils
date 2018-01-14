using WatchFace.Parser.Attributes;

namespace WatchFace.Parser.Elements.BasicElements
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