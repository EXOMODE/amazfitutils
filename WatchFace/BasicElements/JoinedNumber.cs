using System;
using System.Collections.Generic;
using WatchFace.Models;

namespace WatchFace.BasicElements
{
    public class JoinedNumber
    {
        public Number Number { get; set; }
        public long DelimiterImageIndex { get; set; }

        public static JoinedNumber Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new JoinedNumber();
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