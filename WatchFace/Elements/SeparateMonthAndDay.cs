using System;
using System.Collections.Generic;
using WatchFace.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class SeparateMonthAndDay
    {
        public Number Month { get; set; }
        public Number Day { get; set; }

        public static SeparateMonthAndDay Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new SeparateMonthAndDay();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.Month = Number.Parse(parameter.Children);
                        break;
                    case 3:
                        result.Day = Number.Parse(parameter.Children);
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}