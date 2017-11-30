namespace WatchFace.Parser.Models.Elements
{
    public class BluetoothElement : SwitchElement
    {
        public BluetoothElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public override bool SwitchState(WatchState state)
        {
            return state.Bluetooth;
        }
    }
}