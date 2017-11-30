using System;
using System.Collections.Generic;
using System.Drawing;
using WatchFace.Parser.Helpers;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class OnelineTemperatureElement : CompositeElement, IDrawable
    {
        public OnelineTemperatureElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public NumberElement Number { get; set; }
        public long MinusImageIndex { get; set; }
        public long DelimiterImageIndex { get; set; }
        public bool AppendDegreesToBoth { get; set; }
        public long? DegreesImageIndex { get; set; }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            if (state.DayTemperature == null || state.NightTemperature == null) return;

            var images = new List<Bitmap>();
            if (state.DayTemperature < 0)
                images.Add(resources[MinusImageIndex]);
            images.AddRange(Number.GetImagesForNumber(resources, Math.Abs(state.DayTemperature.Value)));
            if (AppendDegreesToBoth && DegreesImageIndex != null)
                images.Add(resources[DegreesImageIndex.Value]);

            images.Add(resources[DelimiterImageIndex]);

            if (state.NightTemperature < 0)
                images.Add(resources[MinusImageIndex]);
            images.AddRange(Number.GetImagesForNumber(resources, Math.Abs(state.NightTemperature.Value)));
            if (DegreesImageIndex != null)
                images.Add(resources[DegreesImageIndex.Value]);

            DrawerHelper.DrawImages(drawer, images, Number.Spacing, Number.Alignment, Number.GetBox());
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Number = new NumberElement(parameter, this);
                    return Number;
                case 2:
                    MinusImageIndex = parameter.Value;
                    return new ValueElement(parameter, this);
                case 3:
                    DelimiterImageIndex = parameter.Value;
                    return new ValueElement(parameter, this);
                case 4:
                    AppendDegreesToBoth = parameter.Value > 0;
                    return new ValueElement(parameter, this);
                case 5:
                    DegreesImageIndex = parameter.Value;
                    return new ValueElement(parameter, this);
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}