using System;
using System.Collections.Generic;
using NLog;
using WatchFace.Elements;
using WatchFace.Models;

namespace WatchFace
{
    public class WatchFace
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
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
            Logger.Trace("Rading root");
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            var result = new WatchFace();

            foreach (var resource in descriptor)
            {
                var currentPath = resource.Id.ToString();
                switch (resource.Id)
                {
                    case 2:
                        result.Background = Background.Parse(resource.Children, currentPath);
                        break;
                    case 3:
                        result.Time = Time.Parse(resource.Children, currentPath);
                        break;
                    case 4:
                        result.Activity = Activity.Parse(resource.Children, currentPath);
                        break;
                    case 5:
                        result.Date = Date.Parse(resource.Children, currentPath);
                        break;
                    case 6:
                        result.Weather = Weather.Parse(resource.Children, currentPath);
                        break;
                    case 7:
                        result.Scales = Scales.Parse(resource.Children, currentPath);
                        break;
                    case 8:
                        result.Status = Status.Parse(resource.Children, currentPath);
                        break;
                    case 9:
                        result.Battery = Battery.Parse(resource.Children, currentPath);
                        break;
                    case 10:
                        result.AnalogDialFace = AnalogDialFace.Parse(resource.Children, currentPath);
                        break;
                    default:
                        throw new InvalidParameterException(resource, "");
                }
            }
            return result;
        }
    }
}