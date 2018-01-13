using System;
using System.Drawing;
using WatchFace.Parser.Models.Elements.AnalogDial;

namespace WatchFace.Parser.Models.Elements
{
    public class AnalogDialBitmapElement : ContainerElement
    {
        public AnalogDialBitmapElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public RectangleElement RedrawArea { get; set; }
        public ImageElement Hours { get; set; }
        public ImageElement Minutes { get; set; }
        public ImageElement Seconds { get; set; }
        public ImageElement CenterImage { get; set; }

        private CoordinatesElement Center { get; set; }

        private Point RotatePoint(CoordinatesElement element, double degrees)
        {
            var radians = degrees / 180 * Math.PI;
            var x = element.X * Math.Cos(radians) + element.Y * Math.Sin(radians);
            var y = element.X * Math.Sin(radians) - element.Y * Math.Cos(radians);
            return new Point((int)Math.Floor(x + Center.X), (int)Math.Floor(y + Center.Y));
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    RedrawArea = new RectangleElement(parameter, this, nameof(Hours));
                    return RedrawArea;
                case 2:
                    Hours = new ImageElement(parameter, this, nameof(Hours));
                    return Hours;
                case 3:
                    Minutes = new ImageElement(parameter, this, nameof(Minutes));
                    return Minutes;
                case 4:
                    Seconds = new ImageElement(parameter, this, nameof(Seconds));
                    return Seconds;
                case 5:
                    CenterImage = new ImageElement(parameter, this, nameof(Seconds));
                    return CenterImage;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}