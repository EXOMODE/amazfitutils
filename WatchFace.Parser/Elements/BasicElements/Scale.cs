using System.Collections.Generic;
using WatchFace.Parser.Attributes;

namespace WatchFace.Parser.Elements.BasicElements
{
    public class Scale
    {
        [ParameterId(1)]
        [ParameterImageIndex]
        public long StartImageIndex { get; set; }

        [ParameterId(2)]
        [ParameterImagesCount]
        public List<Coordinates> Segments { get; set; }
    }
}