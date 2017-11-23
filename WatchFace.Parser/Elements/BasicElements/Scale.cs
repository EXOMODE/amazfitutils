using System.Collections.Generic;
using WatchFace.Utils;

namespace WatchFace.Elements.BasicElements
{
    public class Scale
    {
        [RawParameter(Id = 1)]
        public long StartImageIndex { get; set; }

        [RawParameter(Id = 2)]
        public List<Coordinates> Segments { get; set; }
    }
}