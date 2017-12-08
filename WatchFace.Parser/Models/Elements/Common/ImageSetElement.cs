using System.Drawing;

namespace WatchFace.Parser.Models.Elements
{
    public class ImageSetElement : ImageElement
    {
        public ImageSetElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public long ImagesCount { get; set; }

        public void Draw(Graphics drawer, Bitmap[] resources, int index)
        {
            if (index >= ImagesCount) index = (int) ImagesCount - 1;

            var imageIndex = ImageIndex + index;
            drawer.DrawImage(resources[imageIndex], new Point((int) X, (int) Y));
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 4:
                    ImagesCount = parameter.Value;
                    return new ValueElement(parameter, this);
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}