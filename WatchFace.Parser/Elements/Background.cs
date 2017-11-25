using WatchFace.Parser.Elements.BasicElements;
using WatchFace.Parser.Utils;

namespace WatchFace.Parser.Elements
{
    public class Background
    {
        [ParameterId(1)]
        public Image Image { get; set; }
    }
}