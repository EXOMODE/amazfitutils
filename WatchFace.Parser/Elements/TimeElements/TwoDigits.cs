using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements.TimeElements
{
    public class TwoDigits
    {
        [ParameterId(1)]
        public ImageSet Tens { get; set; }

        [ParameterId(2)]
        public ImageSet Ones { get; set; }
    }
}