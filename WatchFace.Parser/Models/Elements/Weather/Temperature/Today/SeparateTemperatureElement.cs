namespace WatchFace.Parser.Models.Elements
{
    public class SeparateTemperatureElement : ContainerElement
    {
        public SeparateTemperatureElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public TodayDayTemperatureElement Day { get; set; }
        public TodayNightTemperatureElement Night { get; set; }
        public CoordinatesElement Unknown3 { get; set; }
        public CoordinatesElement Unknown4 { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Day = new TodayDayTemperatureElement(parameter, this);
                    return Day;
                case 2:
                    Night = new TodayNightTemperatureElement(parameter, this);
                    return Night;
                case 3:
                    Unknown3 = new CoordinatesElement(parameter, this);
                    return Unknown3;
                case 4:
                    Unknown4 = new CoordinatesElement(parameter, this);
                    return Unknown4;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}