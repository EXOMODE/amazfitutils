using WatchFace.Elements.BasicElements;
using WatchFace.Utils;

namespace WatchFace.Elements.DateElements
{
    public class SeparateMonthAndDay
    {
        [RawParameter(Id = 1)]
        public Number Month { get; set; }

        [RawParameter(Id = 3)]
        public Number Day { get; set; }
    }
}