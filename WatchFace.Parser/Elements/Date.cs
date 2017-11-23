using WatchFace.Elements.BasicElements;
using WatchFace.Elements.DateElements;
using WatchFace.Utils;

namespace WatchFace.Elements
{
    public class Date
    {
        [RawParameter(Id = 1)]
        public MonthAndDay MonthAndDay { get; set; }

        [RawParameter(Id = 2)]
        public ImageSet WeekDay { get; set; }
    }
}