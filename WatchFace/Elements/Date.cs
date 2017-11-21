using System;
using System.Collections.Generic;
using WatchFace.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class Date
    {
        public MonthAndDay MonthAndDay { get; set; }
        public ImageSet WeekDay { get; set; }

        public static Date Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Date();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.MonthAndDay = MonthAndDay.Parse(parameter.Children);
                        break;
                    case 2:
                        result.WeekDay = ImageSet.Parse(parameter.Children);
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}