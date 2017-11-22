using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Models;

namespace WatchFace.Elements.WeatherElements
{
    public class TodayTemperature
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public SeparateTemperature Separate { get; set; }
        public OneLineTemperature OneLine { get; set; }

        public static TodayTemperature Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new TodayTemperature();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Separate = SeparateTemperature.Parse(parameter.Children, currentPath);
                        break;
                    case 2:
                        result.OneLine = OneLineTemperature.Parse(parameter.Children, currentPath);
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}