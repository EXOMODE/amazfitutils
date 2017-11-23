using WatchFace.Elements.BasicElements;
using WatchFace.Utils;

namespace WatchFace.Elements
{
    public class Scales
    {
        [RawParameter(Id = 2)]
        public Scale LinearSteps { get; set; }

        [RawParameter(Id = 3)]
        public CircleScale CircleSteps { get; set; }
    }
}