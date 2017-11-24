using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements
{
    public class StepsProgress
    {
        [RawParameter(Id = 1)]
        public Image GoalImage { get; set; }

        [RawParameter(Id = 2)]
        public Scale Linear { get; set; }

        [RawParameter(Id = 3)]
        public CircleScale Circle { get; set; }
    }
}