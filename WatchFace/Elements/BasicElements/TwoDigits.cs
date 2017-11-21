using System;
using System.Collections.Generic;
using WatchFace.Models;

namespace WatchFace.Elements.BasicElements
{
    public class TwoDigits
    {
        public ImageSet Tens { get; set; }
        public ImageSet Ones { get; set; }

        public static TwoDigits Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new TwoDigits();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.Tens = ImageSet.Parse(parameter.Children);
                        break;
                    case 2:
                        result.Ones = ImageSet.Parse(parameter.Children);
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}