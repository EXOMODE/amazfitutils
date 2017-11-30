using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class StepsElement : NumberElement, IDrawable
    {
        public StepsElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            Draw(drawer, resources, state.Steps);
        }
    }
}