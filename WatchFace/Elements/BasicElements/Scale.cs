using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Models;

namespace WatchFace.Elements.BasicElements
{
    public class Scale
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public long StartImageIndex { get; set; }
        public List<Coordinates> Segments { get; set; }

        public static Scale Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Scale {Segments = new List<Coordinates>()};
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.StartImageIndex = parameter.Value;
                        break;
                    case 2:
                        result.Segments.Add(Coordinates.Parse(parameter.Children, currentPath));
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}