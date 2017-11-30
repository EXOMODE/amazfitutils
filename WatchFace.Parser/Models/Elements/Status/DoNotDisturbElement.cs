namespace WatchFace.Parser.Models.Elements
{
    public class DoNotDisturbElement : SwitchElement
    {
        public DoNotDisturbElement(Parameter parameter, Element parent, string name = null) : 
            base(parameter, parent, name) { }

        public override bool SwitchState(WatchState state)
        {
            return state.DoNotDisturb;
        }
    }
}