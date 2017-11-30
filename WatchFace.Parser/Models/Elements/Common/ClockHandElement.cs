using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements.Common
{
    public abstract class ClockHandElement : CompositeElement, IDrawable
    {
        protected ClockHandElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public bool OnlyBorder { get; set; }
        public Color Color { get; set; }
        public CoordinatesElement Center { get; set; }
        public List<CoordinatesElement> Shape { get; set; } = new List<CoordinatesElement>();
        public ImageElement CenterImage { get; set; }

        public abstract void Draw(Graphics drawer, Bitmap[] resources, WatchState state);

        public void Draw(Graphics drawer, Bitmap[] resources, double value, double total)
        {
            var angle = value * 360 / total - 90;
            var points = Shape.Select(point => RotatePoint(point, angle)).ToArray();
            if (OnlyBorder)
            {
                drawer.DrawPolygon(new Pen(Color), points);
            }
            else
            {
                drawer.FillPolygon(new SolidBrush(Color), points);
                drawer.DrawPolygon(new Pen(Color, 1), points);
            }

            CenterImage?.Draw(drawer, resources);
        }

        private Point RotatePoint(CoordinatesElement element, double degrees)
        {
            var radians = degrees / 180 * Math.PI;
            var x = element.X * Math.Cos(radians) + element.Y * Math.Sin(radians);
            var y = element.X * Math.Sin(radians) + element.Y * Math.Cos(radians);
            return new Point((int) Math.Round(x + Center.X), (int) Math.Round(y + Center.Y));
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    OnlyBorder = parameter.Value > 0;
                    return new ValueElement(parameter, this);
                case 2:
                    Color = Color.FromArgb(0xff, Color.FromArgb((int) parameter.Value));
                    return new ValueElement(parameter, this);
                case 3:
                    Center = new CoordinatesElement(parameter, this);
                    return Center;
                case 4:
                    var point = new CoordinatesElement(parameter, this);
                    Shape.Add(point);
                    return point;
                case 5:
                    CenterImage = new ImageElement(parameter, this);
                    return CenterImage;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}