using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.ActivityElements
{
    public class FormattedNumber
    {
        [ParameterId(1)]
        public Number Number { get; set; }

        [ParameterId(2)]
        [ParameterImageIndex]
        public long SuffixImageIndex { get; set; }

        [ParameterId(3)]
        [ParameterImagesCount]
        public long DecimalPointImageIndex { get; set; }
    }
}