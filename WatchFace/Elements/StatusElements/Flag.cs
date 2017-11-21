using System;
using System.Collections.Generic;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements.StatusElements
{
    public class Flag
    {
        public long ImageIndexOn { get; set; }
        public Coordinates Coordinates { get; set; }

        public static Flag Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Flag();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.Coordinates = Coordinates.Parse(parameter.Children);
                        break;
                    case 2:
                        result.ImageIndexOn = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}