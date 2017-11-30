using WatchFace.Parser.Models.Elements.AnalogDial;

namespace WatchFace.Parser.Models.Elements
{
    public class AnalogDialElement : ContainerElement
    {
        public AnalogDialElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public HoursClockHandElement Hours { get; set; }
        public MinutesClockHandElement Minutes { get; set; }
        public SecondsClockHandElement Seconds { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Hours = new HoursClockHandElement(parameter, this, nameof(Hours));
                    return Hours;
                case 2:
                    Minutes = new MinutesClockHandElement(parameter, this, nameof(Minutes));
                    return Minutes;
                case 3:
                    Seconds = new SecondsClockHandElement(parameter, this, nameof(Seconds));
                    return Seconds;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}