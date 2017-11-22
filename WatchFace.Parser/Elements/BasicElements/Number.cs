using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Models;

namespace WatchFace.Elements.BasicElements
{
    public class Number
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public long TopLeftX { get; set; }
        public long TopLeftY { get; set; }
        public long BottomRightX { get; set; }
        public long BottomRightY { get; set; }
        public long Alignment { get; set; }
        public long Unknown6 { get; set; }
        public long ImageIndex { get; set; }
        public long ImagesCount { get; set; }

        public static Number Parse(List<Parameter> descriptor, string path)
        {
            Logger.Trace("Reading {0}", path);
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Number();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.TopLeftX = parameter.Value;
                        break;
                    case 2:
                        result.TopLeftY = parameter.Value;
                        break;
                    case 3:
                        result.BottomRightX = parameter.Value;
                        break;
                    case 4:
                        result.BottomRightY = parameter.Value;
                        break;
                    case 5:
                        result.Alignment = parameter.Value;
                        break;
                    case 6:
                        result.Unknown6 = parameter.Value;
                        break;
                    case 7:
                        result.ImageIndex = parameter.Value;
                        break;
                    case 8:
                        result.ImagesCount = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            return result;
        }
    }
}