using System.Drawing;

namespace WatchFace.Parser.Models.Elements
{
    public class DigitElement : CoordinatesElement
    {
        public DigitElement(Parameter parameter, Element parent, string name) : base(parameter, parent, name) { }

        public long ImageIndex { get; set; }
        public long ImagesCount { get; set; }

        public void Draw(Graphics drawer, Bitmap[] images, int digit)
        {
            if (digit >= ImagesCount) return;
            var imageIndex = ImageIndex + digit;
            drawer.DrawImage(images[imageIndex], new Point((int) X, (int) Y));
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 3:
                    ImageIndex = parameter.Value;
                    return new ValueElement(parameter, this, nameof(ImageIndex));
                case 4:
                    ImagesCount = parameter.Value;
                    return new ValueElement(parameter, this, nameof(ImagesCount));
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}