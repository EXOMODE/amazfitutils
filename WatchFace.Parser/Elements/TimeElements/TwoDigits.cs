using WatchFace.Utils;

namespace WatchFace.Elements.BasicElements
{
    public class TwoDigits
    {
        [RawParameter(Id = 1)]
        public ImageSet Tens { get; set; }

        [RawParameter(Id = 2)]
        public ImageSet Ones { get; set; }
    }
}