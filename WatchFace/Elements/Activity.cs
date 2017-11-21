using System;
using System.Collections.Generic;
using WatchFace.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class Activity
    {
        public Number Calories { get; set; }
        public Number Pulse { get; set; }
        public Number Steps { get; set; }
        public Number StepsGoal { get; set; }
        public FormattedNumber Distance { get; set; }

        public static Activity Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Activity();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.Steps = Number.Parse(parameter.Children);
                        break;
                    case 2:
                        result.StepsGoal = Number.Parse(parameter.Children);
                        break;
                    case 3:
                        result.Calories = Number.Parse(parameter.Children);
                        break;
                    case 4:
                        result.Pulse = Number.Parse(parameter.Children);
                        break;
                    case 5:
                        result.Distance = FormattedNumber.Parse(parameter.Children);
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}