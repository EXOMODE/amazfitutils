namespace WatchFace.Parser.Models.Elements
{
    public class WeatherElement : ContainerElement
    {
        public WeatherElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public WeatherIconsElement WeatherIcons { get; set; }
        public TemperatureElement Temperature { get; set; }
        public AirPollutionElement AirPollution { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    WeatherIcons = new WeatherIconsElement(parameter, this);
                    return WeatherIcons;
                case 2:
                    Temperature = new TemperatureElement(parameter, this);
                    return Temperature;
                case 3:
                    AirPollution = new AirPollutionElement(parameter, this);
                    return AirPollution;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}