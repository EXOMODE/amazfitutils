using WatchFace.Parser.Attributes;

namespace WatchFace.Parser.Elements.Animations
{
    public class Animation
    {
        [ParameterId(1)]
        public AnimationImage AnimationImage { get; set; }

        [ParameterId(2)]
        public long x1 { get; set; }

        [ParameterId(3)]
        public long y1 { get; set; }

        [ParameterId(4)]
        public long Interval { get; set; }
    }
}