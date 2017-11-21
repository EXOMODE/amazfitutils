using System;
using System.Collections.Generic;
using WatchFace.Models;

namespace WatchFace.Elements.BasicElements
{
    public class Coordinates
    {
        public long X { get; set; }
        public long Y { get; set; }

        public static Coordinates Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Coordinates();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.X = parameter.Value;
                        break;
                    case 2:
                        result.Y = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}