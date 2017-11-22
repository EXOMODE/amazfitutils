using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Models;

namespace WatchFace.Elements.BasicElements
{
    public class ImageSet
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public long X { get; set; }
        public long Y { get; set; }
        public long ImageIndex { get; set; }
        public long ImagesCount { get; set; }

        public static ImageSet Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new ImageSet();
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
                        result.ImageIndex = parameter.Value;
                        break;
                    case 4:
                        result.ImagesCount = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            return result;
        }
    }
}