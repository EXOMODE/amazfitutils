using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class CurrentTemperatureElement : TemperatureNumberElement, IDrawable
    {
        public CurrentTemperatureElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            if (state.CurrentTemperature != null)
                Draw(drawer, resources, state.CurrentTemperature.Value);
        }
    }
}