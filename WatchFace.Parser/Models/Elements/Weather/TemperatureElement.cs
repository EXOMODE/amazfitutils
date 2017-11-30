namespace WatchFace.Parser.Models.Elements
{
    public class TemperatureElement : ContainerElement
    {
        public TemperatureElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public CurrentTemperatureElement Current { get; set; }
        public TodayTemperatureElement Today { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Current = new CurrentTemperatureElement(parameter, this);
                    return Current;
                case 2:
                    Today = new TodayTemperatureElement(parameter, this);
                    return Today;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}