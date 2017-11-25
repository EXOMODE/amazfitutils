using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Elements.DateElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements
{
    public class Date
    {
        [ParameterId(1)]
        public MonthAndDay MonthAndDay { get; set; }

        [ParameterId(2)]
        public ImageSet WeekDay { get; set; }
    }
}