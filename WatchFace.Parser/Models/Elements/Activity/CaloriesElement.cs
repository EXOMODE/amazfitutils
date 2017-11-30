using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class CaloriesElement : NumberElement, IDrawable
    {
        public CaloriesElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            Draw(drawer, resources, state.Calories);
        }
    }
}