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

        public AlarmElement Alarm { get; set; }

        public static bool IsBip { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            if (IsBip)
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
                }
            }
            else
            {
                switch (parameter.Id)
                {
                    case 1:
                        DoNotDisturb = new DoNotDisturbElement(parameter, this);
                        return DoNotDisturb;
                    case 2:
                        Unlocked = new UnlockedElement(parameter, this);
                        return Unlocked;
                    case 3:
                        Bluetooth = new BluetoothElement(parameter, this);
                        return Bluetooth;
                    case 4:
                        Battery = new BatteryElement(parameter, this);
                        return Battery;
                }
            }

            return base.CreateChildForParameter(parameter);
        }
    }
}