using System.Collections.Generic;

namespace WatchFace.Parser.Models.Elements
{
    public class WatchFace : ContainerElement
    {
        public WatchFace(IEnumerable<Parameter> parameters) : base(parameters) { }

        public BackgroundElement Background { get; set; }
        public TimeElement Time { get; set; }
        public ActivityElement Activity { get; set; }
        public DateElement Date { get; set; }
        public WeatherElement Weather { get; set; }
        public GoalProgressElement GoalProgress { get; set; }
        public StatusElement Status { get; set; }
        public BatteryElement Battery { get; set; }
        public AnalogDialElement AnalogDial { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 2:
                    Background = new BackgroundElement(parameter);
                    return Background;
                case 3:
                    Time = new TimeElement(parameter);
                    return Time;
                case 4:
                    Activity = new ActivityElement(parameter);
                    return Activity;
                case 5:
                    Date = new DateElement(parameter);
                    return Date;
                case 6:
                    Weather = new WeatherElement(parameter);
                    return Weather;
                case 7:
                    GoalProgress = new GoalProgressElement(parameter);
                    return GoalProgress;
                case 8:
                    Status = new StatusElement(parameter);
                    return Status;
                case 9:
                    Battery = new BatteryElement(parameter);
                    return Battery;
                case 10:
                    AnalogDial = new AnalogDialElement(parameter);
                    return AnalogDial;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}