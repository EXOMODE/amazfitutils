using WatchFace.Elements.BasicElements;
using WatchFace.Utils;

namespace WatchFace.Elements.ActivityElements
{
    public class FormattedNumber
    {
        [RawParameter(Id = 1)]
        public Number Number { get; set; }

        [RawParameter(Id = 2)]
        public long SuffixImageIndex { get; set; }

        [RawParameter(Id = 3)]
        public long DecimalPointImageIndex { get; set; }
    }
}