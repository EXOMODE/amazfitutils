using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.BasicElements;

namespace WatchFace.Parser.Elements
{
    public class PulseProgress
    {
        [ParameterId(1)]
        public Scale Linear { get; set; }
    }
}