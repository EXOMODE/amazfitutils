using WatchFace.Parser.Models.Elements.GoalProgress;

namespace WatchFace.Parser.Models.Elements
{
    public class HeartProgressElement : ContainerElement
    {
        public HeartProgressElement(Parameter parameter, Element parent = null, string name = null)
          : base(parameter, parent, name)
        {
        }

        public LinearHeartProgressElement Linear { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            if (parameter.Id != (byte)1)
                return base.CreateChildForParameter(parameter);
            this.Linear = new LinearHeartProgressElement(parameter, (Element)this, (string)null);
            return (Element)this.Linear;
        }
    }
}