using System.Drawing;

namespace Resources.Image
{
    public class ColorError
    {
        public const byte LowLevel = 0x00;
        public const byte HighLevel = 0xff;
        public const byte Treshold = 0x80;

        public ColorError(Color original)
        {
            var a = NewValue(original.A);
            var r = NewValue(original.R);
            var g = NewValue(original.G);
            var b = NewValue(original.B);

            NewColor = Color.FromArgb(a, r, g, b);

            ErrorA = original.A - a;
            ErrorR = original.R - r;
            ErrorG = original.G - g;
            ErrorB = original.B - b;
        }

        public Color NewColor { get; }
        public int ErrorA { get; }
        public int ErrorR { get; }
        public int ErrorG { get; }
        public int ErrorB { get; }
        public bool IsZero => ErrorA == 0 && ErrorR == 0 && ErrorG == 0 && ErrorB == 0;

        public Color ApplyError(Color color, int part, int total)
        {
            return Color.FromArgb(
                CheckBounds(color.A + ErrorA * part / total),
                CheckBounds(color.R + ErrorR * part / total),
                CheckBounds(color.G + ErrorG * part / total),
                CheckBounds(color.B + ErrorB * part / total)
            );
        }

        private static byte NewValue(byte value)
        {
            return value < Treshold ? LowLevel : HighLevel;
        }

        private static byte CheckBounds(int value)
        {
            return (byte) (value < LowLevel ? LowLevel : (value > HighLevel ? HighLevel : value));
        }
    }
}