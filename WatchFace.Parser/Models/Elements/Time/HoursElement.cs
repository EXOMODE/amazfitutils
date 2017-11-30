using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class HoursElement : TwoDigitsElement, IDrawable
    {
        public HoursElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            var timeElement = (TimeElement) _parent;
            var hours = timeElement.AmPm == null ? state.Time.Hour : state.Time.Hour % 12;
            Draw(drawer, resources, hours);
        }
    }
}