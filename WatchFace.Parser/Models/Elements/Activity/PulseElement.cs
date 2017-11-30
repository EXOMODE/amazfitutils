using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class PulseElement : NumberElement, IDrawable
    {
        public PulseElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            if (state.Pulse != null)
                Draw(drawer, resources, state.Pulse.Value);
        }
    }
}