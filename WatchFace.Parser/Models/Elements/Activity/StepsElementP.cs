using System.Drawing;
using WatchFace.Parser.Helpers;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class StepsElementP : ContainerElement, IDrawable
    {
        public StepsElementP(Parameter parameter, Element parent = null, string name = null)
          : base(parameter, parent, name)
        { }

        public StepsElement Steps { get; set; }
        public ValueElement IconIndex { get; set; }

        public override void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            if (Steps != null && IconIndex != null && IconIndex.Value > 0) Steps.Icon = resources[IconIndex.Value];

            base.Draw(drawer, resources, state);
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Steps = new StepsElement(parameter, this, null);
                    return Steps;
                case 2:
                    IconIndex = new ValueElement(parameter, this, nameof(IconIndex));
                    return IconIndex;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}