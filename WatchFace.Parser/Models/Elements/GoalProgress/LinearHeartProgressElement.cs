using System.Drawing;

namespace WatchFace.Parser.Models.Elements.GoalProgress
{
    public class LinearHeartProgressElement : LinearProgressElement
    {
        public LinearHeartProgressElement(Parameter parameter, Element parent = null, string name = null)
          : base(parameter, parent, name)
        {
        }

        public override void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            if (!state.Pulse.HasValue)
                return;
            this.Draw(drawer, resources, state.Pulse.Value, state.PulseGoal);
        }
    }
}
