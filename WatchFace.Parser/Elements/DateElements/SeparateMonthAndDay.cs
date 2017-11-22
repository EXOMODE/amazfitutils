using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements.DateElements
{
    public class SeparateMonthAndDay
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public Number Month { get; set; }
        public Number Day { get; set; }

        public static SeparateMonthAndDay Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new SeparateMonthAndDay();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Month = Number.Parse(parameter.Children, currentPath);
                        break;
                    case 3:
                        result.Day = Number.Parse(parameter.Children, currentPath);
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}