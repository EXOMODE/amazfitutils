using System.Drawing;

namespace WatchFace.Parser.Models.Elements
{
    public class AnimationImageSetElement : ImageSetElement
    {
        private int indexPointer;

        public AnimationImageSetElement(Parameter parameter, Element parent, string name = null)
          : base(parameter, parent, name)
        {
        }

        public override void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            if ((long)this.indexPointer >= this.ImagesCount)
                this.indexPointer = 0;
            this.Draw(drawer, resources, this.indexPointer++);
        }
    }
}