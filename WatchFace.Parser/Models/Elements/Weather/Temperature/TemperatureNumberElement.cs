using System;
using System.Collections.Generic;
using System.Drawing;
using WatchFace.Parser.Helpers;

namespace WatchFace.Parser.Models.Elements
{
    public class TemperatureNumberElement : CompositeElement
    {
        public TemperatureNumberElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public NumberElement Number { get; set; }
        public long MinusImageIndex { get; set; }
        public long? DegreesImageIndex { get; set; }

        public void Draw(Graphics drawer, Bitmap[] resources, int temperature, CoordinatesElement altCoordinates = null)
        {
            var drawingBox = altCoordinates == null ? Number.GetBox() : Number.GetAltBox(altCoordinates);
            var images = GetImagesForTemperature(resources, temperature);
            DrawerHelper.DrawImages(drawer, images, (int) Number.Spacing, Number.Alignment, drawingBox);
        }

        public List<Bitmap> GetImagesForTemperature(Bitmap[] resources, int temperature)
        {
            var images = new List<Bitmap>();
            if (temperature < 0)
                images.Add(resources[MinusImageIndex]);
            images.AddRange(Number.GetImagesForNumber(resources, Math.Abs(temperature)));
            if (DegreesImageIndex != null)
                images.Add(resources[DegreesImageIndex.Value]);
            return images;
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Number = new NumberElement(parameter, this);
                    return Number;
                case 2:
                    MinusImageIndex = parameter.Value;
                    return new ValueElement(parameter, this);
                case 3:
                    DegreesImageIndex = parameter.Value;
                    return new ValueElement(parameter, this);
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}