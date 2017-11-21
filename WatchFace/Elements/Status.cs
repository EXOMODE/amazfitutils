using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Elements.StatusElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class Status
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public Switch Bluetooth { get; set; }
        public Flag Alarm { get; set; }
        public Flag Lock { get; set; }
        public Flag DoNotDisturb { get; set; }

        public static Status Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Status();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Bluetooth = Switch.Parse(parameter.Children, currentPath);
                        break;
                    case 2:
                        result.Alarm = Flag.Parse(parameter.Children, currentPath);
                        break;
                    case 3:
                        result.Lock = Flag.Parse(parameter.Children, currentPath);
                        break;
                    case 4:
                        result.DoNotDisturb = Flag.Parse(parameter.Children, currentPath);
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}