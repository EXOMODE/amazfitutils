namespace WatchFace.Parser.Models.Elements
{
    public class BackgroundElement : ContainerElement
    {
        public BackgroundElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public ImageElement Image { get; set; }

        public ImageElement FrontImage { get; set; }

        public ImageElement Preview { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Image = new ImageElement(parameter, this, nameof(Image));
                    return Image;

                case 4:
                    FrontImage = new ImageElement(parameter, this, nameof(FrontImage));
                    return FrontImage;

                case 3:
                    Preview = new ImageElement(parameter, this, nameof(Preview));
                    return Preview;

                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}