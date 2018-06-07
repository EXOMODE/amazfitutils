namespace WatchFace.Parser.Models.Elements
{
    public class AirPollutionElement : ContainerElement
    {
        public AirPollutionElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public AirQualityIndexNumberElement Index { get; set; }
        public AirPollutionImageElement Current { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Index = new AirQualityIndexNumberElement(parameter, this);
                    return Index;
                case 2:
                    Current = new AirPollutionImageElement(parameter, this);
                    return Current;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}