using System.Collections.Generic;
using System.Drawing;
using WatchFace.Parser.Helpers;
using WatchFace.Parser.Interfaces;
using WatchFace.Parser.Models.Elements.Common;

namespace WatchFace.Parser.Models.Elements.Battery
{
    public class BatteryNumberElement : CompositeElement, IDrawable
    {
        public BatteryNumberElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }


        public NumberElement Number { get; set; }
        public CircularProgressElement CircularProgress { get; set; }
        public long? PrefixImageIndex { get; set; }
        public long? SuffixImageIndex { get; set; }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            var images = new List<Bitmap>();
            if (PrefixImageIndex != null)
                images.Add(resources[PrefixImageIndex.Value]);
            images.AddRange(Number.GetImagesForNumber(resources, state.BatteryLevel));
            if (SuffixImageIndex != null)
                images.Add(resources[SuffixImageIndex.Value]);

            DrawerHelper.DrawImages(drawer, images, (int)Number.Spacing, Number.Alignment, Number.GetBox());

            CircularProgress?.Draw(drawer, resources, state);
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Number = new NumberElement(parameter, this, nameof(Number));
                    return Number;
                case 2:
                    CircularProgress = new CircularBatteryProgressElement(parameter, this);
                    return CircularProgress;
                case 3:
                    PrefixImageIndex = parameter.Value;
                    return new ValueElement(parameter, this, nameof(PrefixImageIndex));
                case 4:
                    SuffixImageIndex = parameter.Value;
                    return new ValueElement(parameter, this, nameof(SuffixImageIndex));
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}