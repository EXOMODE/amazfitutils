using System.Collections.Generic;
using System.Drawing;
using WatchFace.Parser.Helpers;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements.Activity
{
    public class StepsWithGoalElement : CompositeElement, IDrawable
    {
        public StepsWithGoalElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public NumberElement Number { get; set; }
        public long DelimiterImageIndex { get; set; }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            var images = new List<Bitmap>();
            images.AddRange(Number.GetImagesForNumber(resources, state.Steps));
            images.Add(resources[DelimiterImageIndex]);
            images.AddRange(Number.GetImagesForNumber(resources, state.Goal));
            DrawerHelper.DrawImages(drawer, images, (int)Number.Spacing, Number.Alignment, Number.GetBox());
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Number = new NumberElement(parameter, this, nameof(Number));
                    return Number;
                case 2:
                    DelimiterImageIndex = parameter.Value;
                    return new ValueElement(parameter, this, nameof(DelimiterImageIndex));
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}