using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements
{
    public class Scales
    {
        [RawParameter(Id = 2)]
        public Scale LinearSteps { get; set; }

        [RawParameter(Id = 3)]
        public CircleScale CircleSteps { get; set; }
    }
}