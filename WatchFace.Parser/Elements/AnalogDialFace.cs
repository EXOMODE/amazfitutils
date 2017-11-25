using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.AnalogDialFaceElements;

namespace WatchFace.Parser.Elements
{
    public class AnalogDialFace
    {
        [ParameterId(1)]
        public ClockHand Hours { get; set; }

        [ParameterId(2)]
        public ClockHand Minutes { get; set; }

        [ParameterId(3)]
        public ClockHand Seconds { get; set; }
    }
}