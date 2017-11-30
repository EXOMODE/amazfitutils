namespace WatchFace.Parser.Models.Elements
{
    public class StatusElement : ContainerElement
    {
        public StatusElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public BluetoothElement Bluetooth { get; set; }
        public AlarmElement Alarm { get; set; }
        public UnlockedElement Unlocked { get; set; }
        public DoNotDisturbElement DoNotDisturb { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Bluetooth = new BluetoothElement(parameter, this);
                    return Bluetooth;
                case 2:
                    Alarm = new AlarmElement(parameter, this);
                    return Alarm;
                case 3:
                    Unlocked = new UnlockedElement(parameter, this);
                    return Unlocked;
                case 4:
                    DoNotDisturb = new DoNotDisturbElement(parameter, this);
                    return DoNotDisturb;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}