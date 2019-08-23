namespace WatchFace.Parser.Models.Elements
{
    public class DateElement : ContainerElement
    {
        public DateElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public MonthAndDayElement MonthAndDay { get; set; }
        public WeekDayElement WeekDay { get; set; }

        public AmPmElement DayAmPm { get; set; }

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
                case 3:
                    DayAmPm = new AmPmElement(parameter, this, nameof(DayAmPm));
                    return DayAmPm;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}