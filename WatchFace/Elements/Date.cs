using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Elements.BasicElements;
using WatchFace.Elements.DateElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class Date
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public MonthAndDay MonthAndDay { get; set; }
        public ImageSet WeekDay { get; set; }

        public static Date Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Date();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.MonthAndDay = MonthAndDay.Parse(parameter.Children, currentPath);
                        break;
                    case 2:
                        result.WeekDay = ImageSet.Parse(parameter.Children, currentPath);
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}