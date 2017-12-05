using System.Drawing;

namespace WatchFace.Parser.Models.Elements
{
    public class TwoDigitsElement : CompositeElement
    {
        public TwoDigitsElement(Parameter parameter, Element parent, string name) : base(parameter, parent, name) { }
        public DigitElement Tens { get; set; }
        public DigitElement Ones { get; set; }

        public void Draw(Graphics drawer, Bitmap[] images, int number)
        {
            if (number > 99) number = number % 100;

            Tens?.Draw(drawer, images, number / 10);
            Ones?.Draw(drawer, images, number % 10);
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Tens = new DigitElement(parameter, this, nameof(Tens));
                    return Tens;
                case 2:
                    Ones = new DigitElement(parameter, this, nameof(Ones));
                    return Ones;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}