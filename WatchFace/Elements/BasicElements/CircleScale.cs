using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NLog;
using WatchFace.JsonConverters;
using WatchFace.Models;

namespace WatchFace.Elements.BasicElements
{
    public class CircleScale
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public long CenterX { get; set; }
        public long CenterY { get; set; }
        public long RadiusX { get; set; }
        public long RadiusY { get; set; }
        public long StartAngle { get; set; }
        public long EndAngle { get; set; }
        public long Width { get; set; }

        [JsonConverter(typeof(HexStringJsonConverter))]
        public long Color { get; set; }

        public static CircleScale Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new CircleScale();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.CenterX = parameter.Value;
                        break;
                    case 2:
                        result.CenterY = parameter.Value;
                        break;
                    case 3:
                        result.RadiusX = parameter.Value;
                        break;
                    case 4:
                        result.RadiusY = parameter.Value;
                        break;
                    case 5:
                        result.StartAngle = parameter.Value;
                        break;
                    case 6:
                        result.EndAngle = parameter.Value;
                        break;
                    case 7:
                        result.Width = parameter.Value;
                        break;
                    case 8:
                        result.Color = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            return result;
        }
    }
}