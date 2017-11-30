using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public abstract class SwitchElement : CompositeElement, IDrawable
    {
        protected SwitchElement(Parameter parameter, Element parent, string name) : base(parameter, parent, name) { }
        public CoordinatesElement Coordinates { get; set; }
        public long? ImageIndexOn { get; set; }
        public long? ImageIndexOff { get; set; }

        public void Draw(Graphics drawer, Bitmap[] images, WatchState state)
        {
            var imageIndex = SwitchState(state) ? ImageIndexOn : ImageIndexOff;
            if (imageIndex == null) return;

            drawer.DrawImage(images[imageIndex.Value], new Point((int) Coordinates.X, (int) Coordinates.Y));
        }

        public abstract bool SwitchState(WatchState state);

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Coordinates = new CoordinatesElement(parameter, this, nameof(Coordinates));
                    return Coordinates;
                case 2:
                    ImageIndexOn = parameter.Value;
                    return new ValueElement(parameter, this, nameof(ImageIndexOn));
                case 3:
                    ImageIndexOff = parameter.Value;
                    return new ValueElement(parameter, this, nameof(ImageIndexOff));
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}