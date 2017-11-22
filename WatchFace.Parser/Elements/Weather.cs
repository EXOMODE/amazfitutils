using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Elements.WeatherElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class Weather
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public WeatherIcon Icon { get; set; }
        public Temperature Temperature { get; set; }
        public AirPollution AirPollution { get; set; }

        public static Weather Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Weather();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Icon = WeatherIcon.Parse(parameter.Children, currentPath);
                        break;
                    case 2:
                        result.Temperature = Temperature.Parse(parameter.Children, currentPath);
                        break;
                    case 3:
                        result.AirPollution = AirPollution.Parse(parameter.Children, currentPath);
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}