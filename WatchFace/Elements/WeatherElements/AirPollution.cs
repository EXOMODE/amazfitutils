using System;
using System.Collections.Generic;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements.WeatherElements
{
    public class AirPollution
    {
        public ImageSet Icon { get; set; }

        public static AirPollution Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new AirPollution();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 2:
                        result.Icon = ImageSet.Parse(parameter.Children);
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}