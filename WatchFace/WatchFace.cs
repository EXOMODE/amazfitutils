using System;
using System.Collections.Generic;
using WatchFace.Elements;
using WatchFace.Models;

namespace WatchFace
{
    public class WatchFace
    {
        public Background Background { get; set; }
        public Time Time { get; set; }
        public Date Date { get; set; }
        public Weather Weather { get; set; }
        public Activity Activity { get; set; }
        public Scales Scales { get; set; }
        public Status Status { get; set; }
        public Battery Battery { get; set; }
        public AnalogDialFace AnalogDialFace { get; set; }

        public static WatchFace Parse(List<Parameter> descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new WatchFace();

            foreach (var resource in descriptor)
                switch (resource.Id)
                {
                    case 2:
                        result.Background = Background.Parse(resource.Children);
                        break;
                    case 3:
                        result.Time = Time.Parse(resource.Children);
                        break;
                    case 4:
                        result.Activity = Activity.Parse(resource.Children);
                        break;
                    case 5:
                        result.Date = Date.Parse(resource.Children);
                        break;
                    case 6:
                        result.Weather = Weather.Parse(resource.Children);
                        break;
                    case 7:
                        result.Scales = Scales.Parse(resource.Children);
                        break;
                    case 8:
                        result.Status = Status.Parse(resource.Children);
                        break;
                    case 9:
                        result.Battery = Battery.Parse(resource.Children);
                        break;
                    case 10:
                        result.AnalogDialFace = AnalogDialFace.Parse(resource.Children);
                        break;
                    default:
                        throw new InvalidParameterException(resource);
                }
            return result;
        }
    }
}