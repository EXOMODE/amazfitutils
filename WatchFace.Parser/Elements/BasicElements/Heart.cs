using WatchFace.Parser.Attributes;

namespace WatchFace.Parser.Elements.BasicElements
{
    public class Heart
    {
        [ParameterId(1)]
        public Scale Scale { get; set; }
    }
}