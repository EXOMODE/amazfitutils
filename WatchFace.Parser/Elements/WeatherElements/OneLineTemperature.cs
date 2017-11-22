using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements.WeatherElements
{
    public class OneLineTemperature
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public Number Number { get; set; }
        public long MinusSignImageIndex { get; set; }
        public long DelimiterImageIndex { get; set; }
        public long Unknown4 { get; set; }
        public long DegreesImageIndex { get; set; }

        public static OneLineTemperature Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new OneLineTemperature();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Number = Number.Parse(parameter.Children, currentPath);
                        break;
                    case 2:
                        result.MinusSignImageIndex = parameter.Value;
                        break;
                    case 3:
                        result.DelimiterImageIndex = parameter.Value;
                        break;
                    case 4:
                        result.Unknown4 = parameter.Value;
                        break;
                    case 5:
                        result.DegreesImageIndex = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}