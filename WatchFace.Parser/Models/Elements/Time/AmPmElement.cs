using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class AmPmElement : CoordinatesElement, IDrawable
    {
        public AmPmElement(Parameter parameter, Element parent, string name) : base(parameter, parent, name) { }
        public long ImageIndexAMCN { get; set; }
        public long ImageIndexPMCN { get; set; }
        public long ImageIndexAMEN { get; set; }
        public long ImageIndexPMEN { get; set; }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            var imageIndex = state.Time.Hour < 12 ? ImageIndexAMEN : ImageIndexPMEN;
            drawer.DrawImage(resources[imageIndex], new Point((int) X, (int) Y));
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 3:
                    ImageIndexAMCN = parameter.Value;
                    return new ValueElement(parameter, this, nameof(ImageIndexAMCN));
                case 4:
                    ImageIndexPMCN = parameter.Value;
                    return new ValueElement(parameter, this, nameof(ImageIndexPMCN));
                case 5:
                    ImageIndexAMEN = parameter.Value;
                    return new ValueElement(parameter, this, nameof(ImageIndexAMEN));
                case 6:
                    ImageIndexPMEN = parameter.Value;
                    return new ValueElement(parameter, this, nameof(ImageIndexPMEN));
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}