using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.BasicElements
{
    public class ImageSet
    {
        [RawParameter(Id = 1)]
        public long X { get; set; }

        [RawParameter(Id = 2)]
        public long Y { get; set; }

        [RawParameter(Id = 3)]
        public long ImageIndex { get; set; }

        [RawParameter(Id = 4)]
        public long ImagesCount { get; set; }
    }
}