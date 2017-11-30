using System.Drawing;
using WatchFace.Parser.Models.Elements.Common;

namespace WatchFace.Parser.Models.Elements.AnalogDial
{
    public class HoursClockHandElement : ClockHandElement
    {
        public HoursClockHandElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public override void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            Draw(drawer, resources, state.Time.Hour % 12 + (double) state.Time.Minute / 60, 12);
        }
    }
}