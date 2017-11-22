using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Models;

namespace WatchFace.Elements.BasicElements
{
    public class TwoDigits
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public ImageSet Tens { get; set; }
        public ImageSet Ones { get; set; }

        public static TwoDigits Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new TwoDigits();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Tens = ImageSet.Parse(parameter.Children, currentPath);
                        break;
                    case 2:
                        result.Ones = ImageSet.Parse(parameter.Children, currentPath);
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}