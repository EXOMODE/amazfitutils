using WatchFace.Elements.BasicElements;
using WatchFace.Utils;

namespace WatchFace.Elements.DateElements
{
    public class OneLineMonthAndDay
    {
        [RawParameter(Id = 1)]
        public Number Number { get; set; }

        [RawParameter(Id = 2)]
        public long DelimiterImageIndex { get; set; }
    }
}