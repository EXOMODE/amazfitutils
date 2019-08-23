using WatchFace.Parser.Attributes;
using WatchFace.Parser.Elements.AnalogDialFaceElements;

namespace WatchFace.Parser.Elements
{
    public class AnalogDialFaceVerge
    {
        [ParameterId(1)]
        public ClockHandVerge Hours { get; set; }

        [ParameterId(2)]
        public ClockHandVerge Minutes { get; set; }

        [ParameterId(3)]
        public ClockHandVerge Seconds { get; set; }
    }
}