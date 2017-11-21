using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class Scales
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public Scale LinearSteps { get; set; }
        public CircleScale CircleSteps { get; set; }

        public static Scales Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Scales();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 2:
                        result.LinearSteps = Scale.Parse(parameter.Children, currentPath);
                        break;
                    case 3:
                        result.CircleSteps = CircleScale.Parse(parameter.Children, currentPath);
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}