using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class TodayNightTemperatureElement : TemperatureNumberElement, IDrawable
    {
        public TodayNightTemperatureElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            if (state.NightTemperature != null)
                Draw(drawer, resources, state.NightTemperature.Value);
        }
    }
}