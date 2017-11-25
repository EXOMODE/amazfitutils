using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements.ActivityElements
{
    public class FormattedNumber
    {
        [ParameterId(1)]
        public Number Number { get; set; }

        [ParameterId(2)]
        [ParameterImageIndex]
        public long? SuffixImageIndex { get; set; }

        [ParameterId(3)]
        [ParameterImageIndex]
        public long? DecimalPointImageIndex { get; set; }
    }
}