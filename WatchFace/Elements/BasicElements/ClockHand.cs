using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WatchFace.JsonConverters;
using WatchFace.Models;

namespace WatchFace.Elements.BasicElements
{
    public class ClockHand
    {
        public long Unknown { get; set; }

        [JsonConverter(typeof(HexStringJsonConverter))]
        public long Color { get; set; }

        public Coordinates Center { get; set; }
        public List<Coordinates> Shape { get; set; }
        public Image CenterImage { get; set; }

        public static ClockHand Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new ClockHand {Shape = new List<Coordinates>()};
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.Unknown = parameter.Value;
                        break;
                    case 2:
                        result.Color = parameter.Value;
                        break;
                    case 3:
                        result.Center = Coordinates.Parse(parameter.Children);
                        break;
                    case 4:
                        result.Shape.Add(Coordinates.Parse(parameter.Children));
                        break;
                    case 5:
                        result.CenterImage = Image.Parse(parameter.Children);
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}