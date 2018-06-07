using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class AirQualityIndexNumberElement : NumberElement, IDrawable
    {
        public AirQualityIndexNumberElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            if (state.AirQualityIndex != null)
                Draw(drawer, resources, state.AirQualityIndex.Value);
        }
    }
}