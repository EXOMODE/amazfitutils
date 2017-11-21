using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class Battery
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public Number Text { get; set; }
        public ImageSet Icon { get; set; }
        public Scale Scale { get; set; }

        public static Battery Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Battery();
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Text = Number.Parse(parameter.Children, currentPath);
                        break;
                    case 2:
                        result.Icon = ImageSet.Parse(parameter.Children, currentPath);
                        break;
                    case 3:
                        result.Scale = Scale.Parse(parameter.Children, currentPath);
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}