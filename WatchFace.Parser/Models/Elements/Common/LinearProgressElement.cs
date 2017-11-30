using System.Collections.Generic;
using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements.GoalProgress
{
    public abstract class LinearProgressElement : CompositeElement, IDrawable
    {
        protected LinearProgressElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public long ImageIndex { get; set; }
        public List<CoordinatesElement> Sectors { get; set; } = new List<CoordinatesElement>();

        public abstract void Draw(Graphics drawer, Bitmap[] resources, WatchState state);

        public void Draw(Graphics drawer, Bitmap[] resources, int value, int total)
        {
            var showSectors = Sectors.Count * value / total;

            for (var i = 0; i < showSectors && i < Sectors.Count; i++)
                drawer.DrawImage(resources[ImageIndex + i], Sectors[i].X, Sectors[i].Y);
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    ImageIndex = parameter.Value;
                    return new ValueElement(parameter, this);
                case 2:
                    var sector = new CoordinatesElement(parameter, this);
                    Sectors.Add(sector);
                    return sector;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}