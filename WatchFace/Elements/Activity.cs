using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Elements.ActivityElements;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class Activity
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public Number Calories { get; set; }
        public Number Pulse { get; set; }
        public Number Steps { get; set; }
        public Number StepsGoal { get; set; }
        public FormattedNumber Distance { get; set; }

        public static Activity Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Activity();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Steps = Number.Parse(parameter.Children, currentPath);
                        break;
                    case 2:
                        result.StepsGoal = Number.Parse(parameter.Children, currentPath);
                        break;
                    case 3:
                        result.Calories = Number.Parse(parameter.Children, currentPath);
                        break;
                    case 4:
                        result.Pulse = Number.Parse(parameter.Children, currentPath);
                        break;
                    case 5:
                        result.Distance = FormattedNumber.Parse(parameter.Children, currentPath);
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}