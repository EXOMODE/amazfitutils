namespace WatchFace.Parser.Models.Elements
{
    public class AlarmElement : SwitchElement
    {
        public AlarmElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public override bool SwitchState(WatchState state)
        {
            return state.Alarm;
        }
    }
}