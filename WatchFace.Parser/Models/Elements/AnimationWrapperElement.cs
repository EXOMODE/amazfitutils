namespace WatchFace.Parser.Models.Elements
{
    public class AnimationWrapperElement : ContainerElement
    {
        public AnimationWrapperElement(Parameter parameter, Element parent = null, string name = null)
          : base(parameter, parent, name)
        {
        }

        public AnimationImageSetElement AnimationImage { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            if (parameter.Id != (byte)1)
                return base.CreateChildForParameter(parameter);
            this.AnimationImage = new AnimationImageSetElement(parameter, (Element)this, (string)null);
            return (Element)this.AnimationImage;
        }
    }
}