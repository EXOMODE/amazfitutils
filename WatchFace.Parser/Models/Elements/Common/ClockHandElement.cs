using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        public static Point GlobalCenter { get; set; }

        public abstract void Draw(Graphics drawer, Bitmap[] resources, WatchState state);

        public void Draw(Graphics drawer, Bitmap[] resources, double value, double total)
        {
            var angle = value * 360 / total;

            if (Center == null || (Center.X == 0 && Center.Y == 0))
            {
                Center.X = GlobalCenter.X;
                Center.Y = GlobalCenter.Y;
            }

            var points = Shape.Select(point => RotatePoint(point, angle)).ToArray();

            if (points.Length > 1)
            {
                if (OnlyBorder)
                {
                    drawer.DrawPolygon(new Pen(Color), points);
                }
                else
                {
                    drawer.FillPolygon(new SolidBrush(Color), points, FillMode.Alternate);
                    drawer.DrawPolygon(new Pen(Color, 1), points);
                }

                CenterImage?.Draw(drawer, resources);
            }
            else if (CenterImage != null)
            {
                Bitmap img = resources[CenterImage.ImageIndex];

                drawer.TranslateTransform(Center.X, Center.Y);
                drawer.RotateTransform((float)angle);
                drawer.TranslateTransform(-Center.X, -Center.Y);

                drawer.DrawImage(img, Center.X - CenterImage.X, Center.Y - CenterImage.Y);
                drawer.ResetTransform();

                return;
            }
        }

        private Point RotatePoint(CoordinatesElement element, double degrees) => RotatePoint(element.X, element.Y, degrees);

        private Point RotatePoint(Point point, double degrees) => RotatePoint(point.X, point.Y, degrees);

        private Point RotatePoint(long pX, long pY, double degrees)
        {
            var radians = degrees / 180 * Math.PI;
            var x = pX * Math.Cos(radians) + pY * Math.Sin(radians);
            var y = pX * Math.Sin(radians) - pY * Math.Cos(radians);
            return new Point((int)Math.Floor(x + Center.X), (int)Math.Floor(y + Center.Y));
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