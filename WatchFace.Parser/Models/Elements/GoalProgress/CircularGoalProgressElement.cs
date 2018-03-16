using System.Drawing;
using WatchFace.Parser.Models.Elements.Common;

namespace WatchFace.Parser.Models.Elements.GoalProgress
{
    public class CircularGoalProgressElement : CircularProgressElement
    {
        public CircularGoalProgressElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public override void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            Draw(drawer, resources, state.Steps, state.Goal);
        }
    }
}