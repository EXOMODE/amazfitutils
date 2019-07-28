namespace WatchFace.Parser.Models.Elements
{
    public class OtherElement : ContainerElement
    {
        public OtherElement(Parameter parameter, Element parent = null, string name = null)
          : base(parameter, parent, name)
        {
        }

        public AnimationWrapperElement AnimationFrames { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            if (parameter.Id != (byte)1)
                return base.CreateChildForParameter(parameter);
            this.AnimationFrames = new AnimationWrapperElement(parameter, (Element)this, (string)null);
            return (Element)this.AnimationFrames;
        }
    }
}