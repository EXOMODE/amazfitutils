using System.Drawing;

namespace WatchFace.Parser.Models.Elements
{
    public class RectangleElement : CoordinatesElement
    {
        public RectangleElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public long BottomRightX { get; set; }
        public long BottomRightY { get; set; }

        public Rectangle GetBox()
        {
            return new Rectangle((int) X, (int) Y, (int) (BottomRightX - X), (int) (BottomRightY - Y));
        }

        public Rectangle GetAltBox(CoordinatesElement altCoordinates)
        {
            return new Rectangle((int) altCoordinates.X, (int) altCoordinates.Y, (int) (BottomRightX - X), (int) (BottomRightY - Y));
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
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}