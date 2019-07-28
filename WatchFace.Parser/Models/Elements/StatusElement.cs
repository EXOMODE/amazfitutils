namespace WatchFace.Parser.Models.Elements
{
    public class StatusElement : ContainerElement
    {
        public StatusElement(Parameter parameter, Element parent = null, string name = null)
          : base(parameter, parent, name)
        {
        }

        public BluetoothElement Bluetooth { get; set; }

        public UnlockedElement Unlocked { get; set; }

        public DoNotDisturbElement DoNotDisturb { get; set; }

        public BatteryElement Battery { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    this.DoNotDisturb = new DoNotDisturbElement(parameter, (Element)this, (string)null);
                    return (Element)this.DoNotDisturb;
                case 2:
                    this.Unlocked = new UnlockedElement(parameter, (Element)this, (string)null);
                    return (Element)this.Unlocked;
                case 3:
                    this.Bluetooth = new BluetoothElement(parameter, (Element)this, (string)null);
                    return (Element)this.Bluetooth;
                case 4:
                    this.Battery = new BatteryElement(parameter, (Element)this, (string)null);
                    return (Element)this.Battery;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}