using System;
using System.Collections.Generic;
using WatchFace.Models;

namespace WatchFace.Elements.WeatherElements
{
    public class Temperature
    {
        public TextTemperature Text { get; set; }

        public static Temperature Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Temperature();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 2:
                        result.Text = TextTemperature.Parse(parameter.Children);
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}