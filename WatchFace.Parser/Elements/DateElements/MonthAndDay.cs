using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.DateElements
{
    public class MonthAndDay
    {
        [RawParameter(Id = 1)]
        public SeparateMonthAndDay Separate { get; set; }

        [RawParameter(Id = 2)]
        public OneLineMonthAndDay OneLine { get; set; }

        [RawParameter(Id = 3)]
        public long Unknown3 { get; set; }

        [RawParameter(Id = 4)]
        public long Unknown4 { get; set; }
    }
}