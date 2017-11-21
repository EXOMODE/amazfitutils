using System;
using System.Collections.Generic;
using WatchFace.Models;

namespace WatchFace.Elements.BasicElements
{
    public class Number
    {
        public long TopLeftX { get; set; }
        public long TopLeftY { get; set; }
        public long BottomRightX { get; set; }
        public long BottomRightY { get; set; }
        public long Unknown1 { get; set; }
        public long Unknown2 { get; set; }
        public long ImageIndex { get; set; }
        public long ImagesCount { get; set; }

        public static Number Parse(List<Parameter> descriptor)
        {
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
                        result.Unknown1 = parameter.Value;
                        break;
                    case 6:
                        result.Unknown2 = parameter.Value;
                        break;
                    case 7:
                        result.ImageIndex = parameter.Value;
                        break;
                    case 8:
                        result.ImagesCount = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}