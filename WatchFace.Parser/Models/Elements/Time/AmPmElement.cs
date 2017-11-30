using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class AmPmElement : CoordinatesElement, IDrawable
    {
        public AmPmElement(Parameter parameter, Element parent, string name) : base(parameter, parent, name) { }
        public long ImageIndexAm { get; set; }
        public long ImageIndexPm { get; set; }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            var imageIndex = state.Time.Hour < 12 ? ImageIndexAm : ImageIndexPm;
            drawer.DrawImage(resources[imageIndex], new Point((int) X, (int) Y));
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 3:
                    ImageIndexAm = parameter.Value;
                    return new ValueElement(parameter, this, nameof(ImageIndexAm));
                case 4:
                    ImageIndexPm = parameter.Value;
                    return new ValueElement(parameter, this, nameof(ImageIndexPm));
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}