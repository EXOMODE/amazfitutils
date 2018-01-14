using System;
using System.Drawing;
using System.Drawing.Imaging;
using BumpKit;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class AnalogDialBitmapElement : CompositeElement, IDrawable
    {
        public AnalogDialBitmapElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public RectangleElement RedrawArea { get; set; }
        public ImageElement Hours { get; set; }
        public ImageElement Minutes { get; set; }
        public ImageElement Seconds { get; set; }
        public ImageElement CenterImage { get; set; }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            if (Hours != null)
            {
                var angle = DegreeFromValues(state.Time.Hour % 12 + (double) state.Time.Minute / 60, 12);
                var hoursImage = RotateImage(
                    resources[Hours.ImageIndex], new Point((int) Hours.X, (int) Hours.Y), angle
                );
                drawer.DrawImage(hoursImage, new Point((int) RedrawArea.X, (int) RedrawArea.Y));
            }

            if (Minutes != null)
            {
                var angle = DegreeFromValues(state.Time.Minute, 60);
                var hoursImage = RotateImage(
                    resources[Minutes.ImageIndex], new Point((int)Minutes.X, (int)Minutes.Y), angle
                );
                drawer.DrawImage(hoursImage, new Point((int)RedrawArea.X, (int)RedrawArea.Y));
            }

            if (Seconds != null)
            {
                var angle = DegreeFromValues(state.Time.Second, 60);
                var hoursImage = RotateImage(
                    resources[Seconds.ImageIndex], new Point((int)Seconds.X, (int)Seconds.Y), angle
                );
                drawer.DrawImage(hoursImage, new Point((int)RedrawArea.X, (int)RedrawArea.Y));
            }

            if (CenterImage != null)
                drawer.DrawImage(resources[CenterImage.ImageIndex], new Point((int)CenterImage.X, (int)CenterImage.Y));
        }

        private static double DegreeFromValues(double value, double total)
        {
            return value * 360 / total - 90;
        }

        private Bitmap RotateImage(Image image, Point ImageCoords, double degrees)
        {
            var radians = degrees / 180 * Math.PI;
            var sin = Math.Sin(radians);
            var cos = Math.Cos(radians);

            var drawBox = RedrawArea.GetBox();
            var newImage = new Bitmap(drawBox.Width, drawBox.Height);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, new Point(ImageCoords.X - drawBox.X, ImageCoords.Y - drawBox.Y));
            }

            while (degrees < 0) degrees += 360;
            while (degrees > 360) degrees -= 360;
            var rotated = newImage.Rotate(degrees);
            rotated.Save("tmp.png");
            return rotated as Bitmap; 
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