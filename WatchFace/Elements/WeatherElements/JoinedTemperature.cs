using System;
using System.Collections.Generic;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements.WeatherElements
{
    public class JoinedTemperature
    {
        public Number Number { get; set; }
        public long MinusSignImageIndex { get; set; }
        public long DelimiterImageIndex { get; set; }
        public long Unknown { get; set; }
        public long DegreesImageIndex { get; set; }

        public static JoinedTemperature Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new JoinedTemperature();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.Number = Number.Parse(parameter.Children);
                        break;
                    case 2:
                        result.MinusSignImageIndex = parameter.Value;
                        break;
                    case 3:
                        result.DelimiterImageIndex = parameter.Value;
                        break;
                    case 4:
                        result.Unknown = parameter.Value;
                        break;
                    case 5:
                        result.DegreesImageIndex = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}