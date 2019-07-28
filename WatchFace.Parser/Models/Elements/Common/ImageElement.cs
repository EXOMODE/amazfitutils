using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class ImageElement : CoordinatesElement, IDrawable
    {
        public ImageElement(Parameter parameter, Element parent, string name = null) : base(parameter, parent, name) { }

        public long ImageIndex { get; set; }

        public virtual void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            Draw(drawer, resources);
        }

        public void Draw(Graphics drawer, Bitmap[] images)
        {
            if (!parameter.IsNoDraw) drawer.DrawImage(images[ImageIndex], new Point((int) X, (int) Y));
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 3:
                    ImageIndex = parameter.Value;
                    return new ValueElement(parameter, this, nameof(ImageIndex));
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}