using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements
{
    public class StepsProgress
    {
        [ParameterId(1)]
        public Image GoalImage { get; set; }

        [ParameterId(2)]
        public Scale Linear { get; set; }

        [ParameterId(3)]
        public CircleScale Circle { get; set; }
    }
}