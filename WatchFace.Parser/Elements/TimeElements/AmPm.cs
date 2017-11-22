using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Models;

namespace WatchFace.Elements.TimeElements
{
    public class AmPm
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public long ImageIndexPm { get; set; }
        public long ImageIndexAm { get; set; }
        public long X { get; set; }
        public long Y { get; set; }

        public static AmPm Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new AmPm();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.X = parameter.Value;
                        break;
                    case 2:
                        result.Y = parameter.Value;
                        break;
                    case 3:
                        result.ImageIndexAm = parameter.Value;
                        break;
                    case 4:
                        result.ImageIndexPm = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            return result;
        }
    }
}