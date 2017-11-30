namespace WatchFace.Parser.Models.Elements
{
    public class DateElement : ContainerElement
    {
        public DateElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public MonthAndDayElement MonthAndDay { get; set; }
        public WeekDayElement WeekDay { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    MonthAndDay = new MonthAndDayElement(parameter, this);
                    return MonthAndDay;
                case 2:
                    WeekDay = new WeekDayElement(parameter, this);
                    return WeekDay;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}