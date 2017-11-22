using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class AnalogDialFace
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public ClockHand Hours { get; set; }
        public ClockHand Minutes { get; set; }
        public ClockHand Seconds { get; set; }

        public static AnalogDialFace Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new AnalogDialFace();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Hours = ClockHand.Parse(parameter.Children, currentPath);
                        break;
                    case 2:
                        result.Minutes = ClockHand.Parse(parameter.Children, currentPath);
                        break;
                    case 3:
                        result.Seconds = ClockHand.Parse(parameter.Children, currentPath);
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}