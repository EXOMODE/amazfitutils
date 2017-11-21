using System;
using System.Collections.Generic;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class Scales
    {
        public Scale Steps { get; set; }

        public static Scales Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Scales();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 2:
                        result.Steps = Scale.Parse(parameter.Children);
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}