namespace WatchFace.Parser.Models.Elements
{
    public class BackgroundElement : ContainerElement
    {
        public BackgroundElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public ImageElement Image { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Image = new ImageElement(parameter, this, nameof(Image));
                    return Image;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}