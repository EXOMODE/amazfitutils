using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class AmPmElement : CoordinatesElement, IDrawable
    {
        public AmPmElement(Parameter parameter, Element parent, string name) : base(parameter, parent, name) { }
        public long ImageIndexAm { get; set; }
        public long PmX { get; set; }
        public long PmY { get; set; }
        public long ImageIndexPm { get; set; }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            if (state.Time.Hour < 12)
                drawer.DrawImage(resources[ImageIndexAm], new Point((int)X, (int)Y));
            else
                drawer.DrawImage(resources[ImageIndexPm], new Point((int)PmX, (int)PmY));
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 3:
                    ImageIndexAm = parameter.Value;
                    return new ValueElement(parameter, this, nameof(ImageIndexAm));
                case 4:
                    PmX = parameter.Value;
                    return new ValueElement(parameter, this, nameof(PmX));
                case 5:
                    PmY = parameter.Value;
                    return new ValueElement(parameter, this, nameof(PmY));
                case 6:
                    ImageIndexPm = parameter.Value;
                    return new ValueElement(parameter, this, nameof(ImageIndexPm));
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}