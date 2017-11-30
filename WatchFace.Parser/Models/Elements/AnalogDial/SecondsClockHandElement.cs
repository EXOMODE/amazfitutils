using System.Drawing;
using WatchFace.Parser.Models.Elements.Common;

namespace WatchFace.Parser.Models.Elements.AnalogDial
{
    public class SecondsClockHandElement : ClockHandElement
    {
        public SecondsClockHandElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public override void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            Draw(drawer, resources, state.Time.Second, 60);
        }
    }
}