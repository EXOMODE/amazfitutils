using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Models;

namespace WatchFace.Elements.BasicElements
{
    public class Coordinates
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public long X { get; set; }
        public long Y { get; set; }

        public static Coordinates Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Coordinates();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.X = parameter.Value;
                        break;
                    case 2:
                        result.Y = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            return result;
        }
    }
}