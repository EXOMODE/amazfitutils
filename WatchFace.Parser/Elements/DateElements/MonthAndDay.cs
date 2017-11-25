using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.DateElements
{
    public class MonthAndDay
    {
        [ParameterId(1)]
        public SeparateMonthAndDay Separate { get; set; }

        [ParameterId(2)]
        public OneLineMonthAndDay OneLine { get; set; }

        [ParameterId(3)]
        public long Unknown3 { get; set; }

        [ParameterId(4)]
        public long Unknown4 { get; set; }
    }
}