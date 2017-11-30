using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class TodayDayTemperatureElement : TemperatureNumberElement, IDrawable
    {
        public TodayDayTemperatureElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            if (state.DayTemperature != null)
                Draw(drawer, resources, state.DayTemperature.Value);
        }
    }
}