using System.Collections.Generic;
using System.Drawing;
using WatchFace.Parser.Helpers;

namespace WatchFace.Parser.Models.Elements
{
    public class CompositeNumberElement : CompositeElement
    {
        public CompositeNumberElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public NumberElement Number { get; set; }
        public long? PrefixImageIndex { get; set; }
        public long? EmptyImageIndex { get; set; }
        public long? SuffixImageIndex { get; set; }

        public void Draw(Graphics drawer, Bitmap[] resources, int? value)
        {
            var images = new List<Bitmap>();
            if (PrefixImageIndex != null)
                images.Add(resources[PrefixImageIndex.Value]);
            if (value == null)
            {
                if (EmptyImageIndex != null) images.Add(resources[EmptyImageIndex.Value]);
            }
            else
                images.AddRange(Number.GetImagesForNumber(resources, value.Value));

            if (SuffixImageIndex != null)
                images.Add(resources[SuffixImageIndex.Value]);

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
                    PrefixImageIndex = parameter.Value;
                    return new ValueElement(parameter, this, nameof(PrefixImageIndex));
                case 3:
                    EmptyImageIndex = parameter.Value;
                    return new ValueElement(parameter, this, nameof(EmptyImageIndex));
                case 4:
                    SuffixImageIndex = parameter.Value;
                    return new ValueElement(parameter, this, nameof(SuffixImageIndex));
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}