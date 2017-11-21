using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements.StatusElements
{
    public class Switch
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public Coordinates Coordinates { get; set; }
        public long ImageIndexOn { get; set; }
        public long ImageIndexOff { get; set; }

        public static Switch Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Switch();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Coordinates = Coordinates.Parse(parameter.Children, currentPath);
                        break;
                    case 2:
                        result.ImageIndexOn = parameter.Value;
                        break;
                    case 3:
                        result.ImageIndexOff = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}