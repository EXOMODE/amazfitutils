using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Elements.DateElements;

namespace WatchFace.Parser.Elements
{
    public class Date
    {
        [ParameterId(1)]
        public MonthAndDay MonthAndDay { get; set; }

        [ParameterId(2)]
        public ImageSet WeekDay { get; set; }

        [ParameterId(3)]
        public DateUnknown3 Unknown3 { get; set; }

        [ParameterId(4)]
        public Coordinates Unknown4 { get; set; }
    }
}