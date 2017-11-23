using System.Collections.Generic;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.BasicElements
{
    public class Scale
    {
        [RawParameter(Id = 1)]
        public long StartImageIndex { get; set; }

        [RawParameter(Id = 2)]
        public List<Coordinates> Segments { get; set; }
    }
}