using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class SecondsElement : TwoDigitsElement, IDrawable
    {
        public SecondsElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            Draw(drawer, resources, state.Time.Second);
        }
    }
}