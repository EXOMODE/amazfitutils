using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WatchFace.Parser.Helpers;

namespace WatchFace.Parser.Models.Elements
{
    public class NumberElement : CoordinatesElement
    {
        public NumberElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public long BottomRightX { get; set; }
        public long BottomRightY { get; set; }
        public TextAlignment Alignment { get; set; }
        public long Spacing { get; set; }
        public long ImageIndex { get; set; }
        public long ImagesCount { get; set; }

        public Rectangle GetBox()
        {
            return new Rectangle((int) X, (int) Y, (int) (BottomRightX - X), (int) (BottomRightY - Y));
        }

        public Rectangle GetAltBox(CoordinatesElement altCoordinates)
        {
            return new Rectangle((int) altCoordinates.X, (int) altCoordinates.Y, (int) (BottomRightX - X), (int) (BottomRightY - Y));
        }

        public void Draw(Graphics drawer, Bitmap[] images, int number, int minimumDigits = 1)
        {
            DrawerHelper.DrawImages(drawer, GetImagesForNumber(images, number, minimumDigits), (int) Spacing, Alignment, GetBox());
        }

        public List<Bitmap> GetImagesForNumber(Bitmap[] images, int number, int minimumDigits = 1)
        {
            var stringNumber = number.ToString().PadLeft(minimumDigits, '0');

            return (from digitChar in stringNumber
                select int.Parse(digitChar.ToString())
                into digit
                where digit < ImagesCount
                select images[ImageIndex + digit]).ToList();
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 3:
                    BottomRightX = parameter.Value;
                    return new ValueElement(parameter, this, nameof(BottomRightX));
                case 4:
                    BottomRightY = parameter.Value;
                    return new ValueElement(parameter, this, nameof(BottomRightY));
                case 5:
                    Alignment = (TextAlignment) parameter.Value;
                    return new ValueElement(parameter, this, nameof(Alignment));
                case 6:
                    Spacing = parameter.Value;
                    return new ValueElement(parameter, this, nameof(Spacing));
                case 7:
                    ImageIndex = parameter.Value;
                    return new ValueElement(parameter, this, nameof(ImageIndex));
                case 8:
                    ImagesCount = parameter.Value;
                    return new ValueElement(parameter, this, nameof(ImagesCount));
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}