using System;
using System.Collections.Generic;
using WatchFace.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class MonthAndDay
    {
        public SeparateMonthAndDay Separate { get; set; }
        public JoinedNumber Joined { get; set; }
        public long Unknown1 { get; set; }
        public long Unknown2 { get; set; }

        public static MonthAndDay Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new MonthAndDay();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.Separate = SeparateMonthAndDay.Parse(parameter.Children);
                        break;
                    case 2:
                        result.Joined = JoinedNumber.Parse(parameter.Children);
                        break;
                    case 3:
                        result.Unknown1 = parameter.Value;
                        break;
                    case 4:
                        result.Unknown2 = parameter.Value;
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}