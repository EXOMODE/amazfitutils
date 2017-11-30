using System.Drawing;
using WatchFace.Parser.Models.Elements.GoalProgress;

namespace WatchFace.Parser.Models.Elements.Battery
{
    public class BatteryLenearProgressElement : LinearProgressElement
    {
        public BatteryLenearProgressElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public override void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            Draw(drawer, resources, state.BatteryLevel, 100);
        }
    }
}