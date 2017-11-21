using System;
using System.Collections.Generic;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class AnalogDialFace
    {
        public ClockHand Hours { get; set; }
        public ClockHand Minutes { get; set; }
        public ClockHand Seconds { get; set; }

        public static AnalogDialFace Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new AnalogDialFace();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.Hours = ClockHand.Parse(parameter.Children);
                        break;
                    case 2:
                        result.Minutes = ClockHand.Parse(parameter.Children);
                        break;
                    case 3:
                        result.Seconds = ClockHand.Parse(parameter.Children);
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}