using System.Drawing;

namespace WatchFace.Parser.Models.Elements
{
    public class AirPollutionImageElement : ImageSetElement
    {
        public AirPollutionImageElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public override void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            if (state.AirQuality == AirCondition.Unknown) return;

            var imageIndex = (int) state.AirQuality;
            Draw(drawer, resources, imageIndex);
        }
    }
}