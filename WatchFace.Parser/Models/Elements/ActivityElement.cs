namespace WatchFace.Parser.Models.Elements
{
    public class ActivityElement : ContainerElement
    {
        public ActivityElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public StepsElement Steps { get; set; }
        public StepsGoalElement StepsGoal { get; set; }
        public CaloriesElement Calories { get; set; }
        public PulseElement Pulse { get; set; }
        public DistanceElement Distance { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Steps = new StepsElement(parameter, this);
                    return Steps;
                case 2:
                    StepsGoal = new StepsGoalElement(parameter, this);
                    return StepsGoal;
                case 3:
                    Calories = new CaloriesElement(parameter, this);
                    return Calories;
                case 4:
                    Pulse = new PulseElement(parameter, this);
                    return Pulse;
                case 5:
                    Distance = new DistanceElement(parameter, this);
                    return Distance;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}