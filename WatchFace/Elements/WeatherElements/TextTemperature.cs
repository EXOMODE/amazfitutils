using System;
using System.Collections.Generic;
using WatchFace.Models;

namespace WatchFace.Elements.WeatherElements
{
    public class TextTemperature
    {
        public SeparateTemperature Separate { get; set; }
        public JoinedTemperature Joined { get; set; }

        public static TextTemperature Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new TextTemperature();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.Separate = SeparateTemperature.Parse(parameter.Children);
                        break;
                    case 2:
                        result.Joined = JoinedTemperature.Parse(parameter.Children);
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}