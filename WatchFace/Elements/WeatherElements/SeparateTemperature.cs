using System;
using System.Collections.Generic;
using WatchFace.Models;

namespace WatchFace.Elements.WeatherElements
{
    public class SeparateTemperature
    {
        public SignedNumber Day { get; set; }
        public SignedNumber Night { get; set; }

        public static SeparateTemperature Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new SeparateTemperature();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.Day = SignedNumber.Parse(parameter.Children);
                        break;
                    case 2:
                        result.Night = SignedNumber.Parse(parameter.Children);
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}