using System;
using System.Collections.Generic;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements.ActivityElements
{
    public class FormattedNumber
    {
        public Number Number { get; set; }
        public long SuffixImageIndex { get; set; }
        public long DecimalPointImageIndex { get; set; }

        public static FormattedNumber Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new FormattedNumber();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.Number = Number.Parse(parameter.Children);
                        break;
                    case 2:
                        result.SuffixImageIndex = parameter.Value;
                        break;
                    case 3:
                        result.DecimalPointImageIndex = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}