using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NLog;
using WatchFace.JsonConverters;
using WatchFace.Models;

namespace WatchFace.Elements.BasicElements
{
    public class ClockHand
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public long Unknown1 { get; set; }

        [JsonConverter(typeof(HexStringJsonConverter))]
        public long Color { get; set; }

        public Coordinates Center { get; set; }
        public List<Coordinates> Shape { get; set; }
        public Image CenterImage { get; set; }

        public static ClockHand Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);

            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new ClockHand {Shape = new List<Coordinates>()};
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Unknown1 = parameter.Value;
                        break;
                    case 2:
                        result.Color = parameter.Value;
                        break;
                    case 3:
                        result.Center = Coordinates.Parse(parameter.Children, currentPath);
                        break;
                    case 4:
                        result.Shape.Add(Coordinates.Parse(parameter.Children, currentPath));
                        break;
                    case 5:
                        result.CenterImage = Image.Parse(parameter.Children, currentPath);
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}