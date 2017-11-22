using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Elements.BasicElements;
using WatchFace.Elements.TimeElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class Time
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public TwoDigits Hours { get; set; }
        public TwoDigits Minutes { get; set; }
        public TwoDigits Seconds { get; set; }
        public AmPm AmPm { get; set; }

        public static Time Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Time();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Hours = TwoDigits.Parse(parameter.Children, currentPath);
                        break;
                    case 2:
                        result.Minutes = TwoDigits.Parse(parameter.Children, currentPath);
                        break;
                    case 3:
                        result.Seconds = TwoDigits.Parse(parameter.Children, currentPath);
                        break;
                    case 4:
                        result.AmPm = AmPm.Parse(parameter.Children, currentPath);
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}