using WatchFace.Elements.BasicElements;
using WatchFace.Utils;

namespace WatchFace.Elements
{
    public class Battery
    {
        [RawParameter(Id = 1)]
        public Number Text { get; set; }

        [RawParameter(Id = 2)]
        public ImageSet Icon { get; set; }

        [RawParameter(Id = 3)]
        public Scale Scale { get; set; }
    }
}