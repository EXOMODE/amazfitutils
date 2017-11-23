using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements.TimeElements
{
    public class TwoDigits
    {
        [RawParameter(Id = 1)]
        public ImageSet Tens { get; set; }

        [RawParameter(Id = 2)]
        public ImageSet Ones { get; set; }
    }
}