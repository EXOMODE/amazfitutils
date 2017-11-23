using WatchFace.Utils;

namespace WatchFace.Elements.BasicElements
{
    public class Number
    {
        [RawParameter(Id = 1)]
        public long TopLeftX { get; set; }

        [RawParameter(Id = 2)]
        public long TopLeftY { get; set; }

        [RawParameter(Id = 3)]
        public long BottomRightX { get; set; }

        [RawParameter(Id = 4)]
        public long BottomRightY { get; set; }

        [RawParameter(Id = 5)]
        public long Alignment { get; set; }

        [RawParameter(Id = 6)]
        public long Unknown6 { get; set; }

        [RawParameter(Id = 7)]
        public long ImageIndex { get; set; }

        [RawParameter(Id = 8)]
        public long ImagesCount { get; set; }
    }
}