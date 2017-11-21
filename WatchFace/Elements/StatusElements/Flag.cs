using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements.StatusElements
{
    public class Flag
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public Coordinates Coordinates { get; set; }
        public long ImageIndex { get; set; }

        public static Flag Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Flag();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Coordinates = Coordinates.Parse(parameter.Children, currentPath);
                        break;
                    case 2:
                        result.ImageIndex = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}