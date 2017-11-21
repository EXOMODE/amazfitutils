using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements.DateElements
{
    public class OneLineMonthAndDay
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public Number Number { get; set; }
        public long DelimiterImageIndex { get; set; }

        public static OneLineMonthAndDay Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new OneLineMonthAndDay();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Number = Number.Parse(parameter.Children, currentPath);
                        break;
                    case 2:
                        result.DelimiterImageIndex = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}