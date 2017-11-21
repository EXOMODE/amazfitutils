using System;
using System.Collections.Generic;
using WatchFace.Models;

namespace WatchFace.BasicElements
{
    public class AmPmSwitch
    {
        public long ImageIndexPm { get; set; }
        public long ImageIndexAm { get; set; }
        public long X { get; set; }
        public long Y { get; set; }

        public static AmPmSwitch Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new AmPmSwitch();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.X = parameter.Value;
                        break;
                    case 2:
                        result.Y = parameter.Value;
                        break;
                    case 3:
                        result.ImageIndexAm = parameter.Value;
                        break;
                    case 4:
                        result.ImageIndexPm = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}