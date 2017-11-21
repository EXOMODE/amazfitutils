using System;
using System.Collections.Generic;
using WatchFace.Models;

namespace WatchFace.Elements.BasicElements
{
    public class Scale
    {
        public long StartImageIndex { get; set; }
        public List<Coordinates> Segments { get; set; }

        public static Scale Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Scale {Segments = new List<Coordinates>()};
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.StartImageIndex = parameter.Value;
                        break;
                    case 2:
                        result.Segments.Add(Coordinates.Parse(parameter.Children));
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}