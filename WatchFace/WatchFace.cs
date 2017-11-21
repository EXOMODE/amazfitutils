using System;
using System.Collections.Generic;
using WatchFace.Elements;
using WatchFace.Models;

namespace WatchFace
{
    public class WatchFace
    {
        public Background Background { get; private set; }
        public Time Time { get; private set; }
        public Date Date { get; private set; }
        public Activity Activity { get; private set; }

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
                    //default:
                    //    throw new InvalidParameterException(resource);
                }
            return result;
        }
    }
}