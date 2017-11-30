using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class ContainerElement : CompositeElement, IDrawable
    {
        public ContainerElement() { }

        public ContainerElement(IEnumerable<Parameter> parameters) : base(parameters) { }

        public ContainerElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public virtual void Draw(Graphics drawer, Bitmap[] images, WatchState state)
        {
            foreach (var element in Children.OfType<IDrawable>())
                element.Draw(drawer, images, state);
        }
    }
}