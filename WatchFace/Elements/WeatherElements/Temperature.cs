using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Models;

namespace WatchFace.Elements.WeatherElements
{
    public class Temperature
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public TemperatureNumber Current { get; set; }
        public TodayTemperature Today { get; set; }

        public static Temperature Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Temperature();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Current = TemperatureNumber.Parse(parameter.Children, currentPath);
                        break;
                    case 2:
                        result.Today = TodayTemperature.Parse(parameter.Children, currentPath);
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}