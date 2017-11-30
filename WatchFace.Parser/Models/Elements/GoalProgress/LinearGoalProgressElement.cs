using System.Drawing;

namespace WatchFace.Parser.Models.Elements.GoalProgress
{
    public class LinearGoalProgressElement : LinearProgressElement
    {
        public LinearGoalProgressElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public override void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            Draw(drawer, resources, state.Steps, state.Goal);
        }
    }
}