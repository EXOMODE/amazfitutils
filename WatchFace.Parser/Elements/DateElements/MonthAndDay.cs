using WatchFace.Parser.Attributes;

namespace WatchFace.Parser.Elements.DateElements
{
    public class MonthAndDay
    {
        [ParameterId(1)]
        public SeparateMonthAndDay Separate { get; set; }

        [ParameterId(2)]
        public OneLineMonthAndDay OneLine { get; set; }

        [ParameterId(3)]
        public bool TwoDigitsMonth { get; set; }

        [ParameterId(4)]
        public bool TwoDigitsDay { get; set; }
    }
}