namespace WatchFace.Parser.Models.Elements
{
    public class TimeElement : ContainerElement
    {
        public TimeElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public HoursElement Hours { get; set; }
        public MinutesElement Minutes { get; set; }
        public SecondsElement Seconds { get; set; }
        public AmPmElement AmPm { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Hours = new HoursElement(parameter, this, nameof(Hours));
                    return Hours;
                case 2:
                    Minutes = new MinutesElement(parameter, this, nameof(Minutes));
                    return Minutes;
                case 3:
                    Seconds = new SecondsElement(parameter, this, nameof(Seconds));
                    return Seconds;
                case 4:
                    AmPm = new AmPmElement(parameter, this, nameof(AmPm));
                    return AmPm;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}