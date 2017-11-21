using System;
using System.Collections.Generic;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements.DateElements
{
    public class OneLineMonthAndDay
    {
        public Number Number { get; set; }
        public long DelimiterImageIndex { get; set; }

        public static OneLineMonthAndDay Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new OneLineMonthAndDay();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.Number = Number.Parse(parameter.Children);
                        break;
                    case 2:
                        result.DelimiterImageIndex = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}