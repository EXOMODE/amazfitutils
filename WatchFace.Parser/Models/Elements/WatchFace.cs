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

        public OtherElement Animation { get; set; }

        public HeartProgressElement HeartProgress { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 2:
                    Background = new BackgroundElement(parameter, null);
                    return Background;
                case 3:
                    Time = new TimeElement(parameter, null);
                    return Time;
                case 4:
                    Activity = new ActivityElement(parameter, null);
                    return Activity;
                case 5:
                    Date = new DateElement(parameter, null);
                    return Date;
                case 6:
                    Weather = new WeatherElement(parameter, null);
                    return Weather;
                case 7:
                    GoalProgress = new GoalProgressElement(parameter, null);
                    return GoalProgress;
                case 8:
                    Status = new StatusElement(parameter, null);
                    return Status;
                case 9:
                    Battery = new BatteryElement(parameter, null);
                    return Status;
                case 10:
                    AnalogDial = new AnalogDialElement(parameter, null);
                    return AnalogDial;
                case 11:
                    Animation = new OtherElement(parameter, null);
                    return Animation;
                case 12:
                    HeartProgress = new HeartProgressElement(parameter, null);
                    return HeartProgress;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}