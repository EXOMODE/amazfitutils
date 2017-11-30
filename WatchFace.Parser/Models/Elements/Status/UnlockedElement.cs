namespace WatchFace.Parser.Models.Elements
{
    public class UnlockedElement : SwitchElement
    {
        public UnlockedElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public override bool SwitchState(WatchState state)
        {
            return state.Unlocked;
        }
    }
}