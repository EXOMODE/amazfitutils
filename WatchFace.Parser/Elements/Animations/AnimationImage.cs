using WatchFace.Parser.Attributes;

namespace WatchFace.Parser.Elements.Animations
{
    public class AnimationImage
    {
        [ParameterId(1)]
        public long X { get; set; }

        [ParameterId(2)]
        public long Y { get; set; }

        [ParameterId(3)]
        [ParameterImageIndex]
        public long ImageIndex { get; set; }

        [ParameterId(4)]
        [ParameterImagesCount]
        public long ImageCount { get; set; }

        [ParameterId(5)]
        public long X3 { get; set; }
    }
}