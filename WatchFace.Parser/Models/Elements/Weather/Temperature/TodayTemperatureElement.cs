namespace WatchFace.Parser.Models.Elements
{
    public class TodayTemperatureElement : ContainerElement
    {
        public TodayTemperatureElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public SeparateTemperatureElement Separate { get; set; }
        public OnelineTemperatureElement Oneline { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Separate = new SeparateTemperatureElement(parameter, this);
                    return Separate;
                case 2:
                    Oneline = new OnelineTemperatureElement(parameter, this);
                    return Oneline;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}