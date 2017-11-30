using System.Drawing;

namespace WatchFace.Parser.Models.Elements.GoalProgress
{
    public class GoalReachedElement : ImageElement
    {
        public GoalReachedElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public override void Draw(Graphics drawer, Bitmap[] images, WatchState state)
        {
            if (state.Steps >= state.Goal) Draw(drawer, images);
        }
    }
}