using System.Drawing;

namespace WatchFace.Parser.Models.Elements
{
    public class SeparateTemperatureElement : ContainerElement
    {
        public SeparateTemperatureElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public TemperatureNumberElement Day { get; set; }
        public TemperatureNumberElement Night { get; set; }
        public CoordinatesElement DayAlt { get; set; }
        public CoordinatesElement NightAlt { get; set; }

        public override void Draw(Graphics drawer, Bitmap[] images, WatchState state)
        {
            if (state.CurrentTemperature != null)
            {
                if (state.DayTemperature != null)
                    Day?.Draw(drawer, images, state.DayTemperature.Value);
                if (state.NightTemperature != null)
                    Night?.Draw(drawer, images, state.NightTemperature.Value);
            }
            else
            {
                if (state.DayTemperature != null)
                    Day?.Draw(drawer, images, state.DayTemperature.Value, DayAlt);
                if (state.NightTemperature != null)
                    Night?.Draw(drawer, images, state.NightTemperature.Value, NightAlt);
            }
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Day = new TemperatureNumberElement(parameter, this);
                    return Day;
                case 2:
                    Night = new TemperatureNumberElement(parameter, this);
                    return Night;
                case 3:
                    DayAlt = new CoordinatesElement(parameter, this);
                    return DayAlt;
                case 4:
                    NightAlt = new CoordinatesElement(parameter, this);
                    return NightAlt;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}