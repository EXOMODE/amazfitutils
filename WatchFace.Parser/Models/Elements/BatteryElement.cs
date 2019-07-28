using System.Drawing;
using WatchFace.Parser.Models.Elements.Battery;

namespace WatchFace.Parser.Models.Elements
{
    public class BatteryElement : ContainerElement
    {
        public BatteryElement(Parameter parameter, Element parent = null, string name = null)
          : base(parameter, parent, name)
        {
        }

        public BatteryNumberElement Text { get; set; }

        public BatteryImageSetElement Icon { get; set; }

        public Element Scale { get; set; }

        public ImageElement Persent { get; set; }

        public override void Draw(Graphics drawer, Bitmap[] images, WatchState state)
        {
            base.Draw(drawer, images, state);
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            if (StatusElement.IsBip)
            {
                switch (parameter.Id)
                {
                    case 1:
                        Text = new BatteryNumberElement(parameter, this);
                        return Text;
                    case 2:
                        Icon = new BatteryImageSetElement(parameter, this);
                        return Icon;
                    case 3:
                        Scale = new BatteryLenearProgressElement(parameter, this);
                        return Scale;
                }
            }
            else
            {
                switch (parameter.Id)
                {
                    case 1:
                    case 2:
                        Text = new BatteryNumberElement(parameter, this);
                        return Text;
                    case 3:
                        Icon = new BatteryImageSetElement(parameter, this);
                        return Icon;
                    case 6:
                        Persent = new ImageElement(parameter, this);
                        return Persent;

                    case 7:
                        Scale = new CircularBatteryProgressElement(parameter, this);
                        return Scale;
                }
            }

            return base.CreateChildForParameter(parameter);
        }
    }
}