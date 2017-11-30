namespace WatchFace.Parser.Models.Elements
{
    public class MonthAndDayElement : ContainerElement
    {
        public MonthAndDayElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public OneLineMonthAndDayElement OneLine { get; set; }
        public SeparateMonthAndDayElement Separate { get; set; }
        public bool TwoDigitsMonth { get; set; }
        public bool TwoDigitsDay { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Separate = new SeparateMonthAndDayElement(parameter, this);
                    return Separate;
                case 2:
                    OneLine = new OneLineMonthAndDayElement(parameter, this);
                    return OneLine;
                case 3:
                    TwoDigitsMonth = parameter.Value == 1;
                    return new ValueElement(parameter, this);
                case 4:
                    TwoDigitsDay = parameter.Value == 1;
                    return new ValueElement(parameter, this);
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}