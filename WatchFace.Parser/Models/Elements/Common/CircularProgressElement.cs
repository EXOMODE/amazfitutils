using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements.GoalProgress
{
    public abstract class CircularProgressElement : CoordinatesElement, IDrawable
    {
        protected CircularProgressElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public long RadiusX { get; set; }
        public long RadiusY { get; set; }
        public long StartAngle { get; set; }
        public long EndAngle { get; set; }
        public long Width { get; set; }
        public Color Color { get; set; }

        public abstract void Draw(Graphics drawer, Bitmap[] resources, WatchState state);

        public void Draw(Graphics drawer, Bitmap[] resources, int value, int total)
        {
            var sectorAngle = (EndAngle - StartAngle) * value / total;
            var pen = new Pen(Color, Width);
            var rect = new Rectangle((int) (X - RadiusX), (int) (Y - RadiusY),
                (int) (RadiusX * 2), (int) (RadiusY * 2));
            drawer.DrawArc(pen, rect, StartAngle - 90, sectorAngle);
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 3:
                    RadiusX = parameter.Value;
                    return new ValueElement(parameter, this);
                case 4:
                    RadiusY = parameter.Value;
                    return new ValueElement(parameter, this);
                case 5:
                    StartAngle = parameter.Value;
                    return new ValueElement(parameter, this);
                case 6:
                    EndAngle = parameter.Value;
                    return new ValueElement(parameter, this);
                case 7:
                    Width = parameter.Value;
                    return new ValueElement(parameter, this);
                case 8:
                    Color = Color.FromArgb(0xff, Color.FromArgb((int) parameter.Value));
                    return new ValueElement(parameter, this);
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}