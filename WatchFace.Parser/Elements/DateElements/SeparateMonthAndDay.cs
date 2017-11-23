using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.DateElements
{
    public class SeparateMonthAndDay
    {
        [RawParameter(Id = 1)]
        public Number Month { get; set; }

        [RawParameter(Id = 3)]
        public Number Day { get; set; }
    }
}