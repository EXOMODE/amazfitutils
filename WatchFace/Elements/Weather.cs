using System;
using System.Collections.Generic;
using WatchFace.Elements.WeatherElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class Weather
    {
        public WeatherIcon Icon { get; set; }
        public Temperature Temperature { get; set; }
        public AirPollution AirPollution { get; set; }

        public static Weather Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Weather();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.Icon = WeatherIcon.Parse(parameter.Children);
                        break;
                    case 2:
                        result.Temperature = Temperature.Parse(parameter.Children);
                        break;
                    case 3:
                        result.AirPollution = AirPollution.Parse(parameter.Children);
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}