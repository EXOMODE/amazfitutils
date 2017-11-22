using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Models;

namespace WatchFace.Elements.DateElements
{
    public class MonthAndDay
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public SeparateMonthAndDay Separate { get; set; }
        public OneLineMonthAndDay Joined { get; set; }
        public long Unknown3 { get; set; }
        public long Unknown4 { get; set; }

        public static MonthAndDay Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new MonthAndDay();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Separate = SeparateMonthAndDay.Parse(parameter.Children, currentPath);
                        break;
                    case 2:
                        result.Joined = OneLineMonthAndDay.Parse(parameter.Children, currentPath);
                        break;
                    case 3:
                        result.Unknown3 = parameter.Value;
                        break;
                    case 4:
                        result.Unknown4 = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}