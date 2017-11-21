using System;
using System.Collections.Generic;
using WatchFace.Elements.StatusElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class Status
    {
        public Switch Bluetooth { get; set; }
        public Flag Alarm { get; set; }
        public Flag Lock { get; set; }
        public Flag DoNotDisturb { get; set; }

        public static Status Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new Status();
            foreach (var parameter in descriptor)
                switch (parameter.Id)
                {
                    case 1:
                        result.Bluetooth = Switch.Parse(parameter.Children);
                        break;
                    case 2:
                        result.Alarm = Flag.Parse(parameter.Children);
                        break;
                    case 3:
                        result.Lock = Flag.Parse(parameter.Children);
                        break;
                    case 4:
                        result.DoNotDisturb = Flag.Parse(parameter.Children);
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}