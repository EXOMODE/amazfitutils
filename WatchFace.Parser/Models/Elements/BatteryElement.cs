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

        public BatteryLenearProgressElement Scale { get; set; }

        public ImageElement Persent { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
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
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}