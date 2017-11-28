using System.Drawing;

namespace Resources.Dithering
{
    public class ColorError
    {
        public const int LowLevel = 0x00;
        public const int HighLevel = 0xff;
        public const int Treshold = 0x80;

        public ColorError(Color original)
        {
            var r = original.R < Treshold ? LowLevel : HighLevel;
            var g = original.G < Treshold ? LowLevel : HighLevel;
            var b = original.B < Treshold ? LowLevel : HighLevel;

            NewColor = Color.FromArgb(original.A, r, g, b);

            ErrorR = original.R - r;
            ErrorG = original.G - g;
            ErrorB = original.B - b;
        }

        public Color NewColor { get; }
        public int ErrorR { get; }
        public int ErrorG { get; }
        public int ErrorB { get; }
        public bool IsZero => ErrorR == 0 && ErrorG == 0 && ErrorB == 0;

        public Color ApplyError(Color color, int part, int total)
        {
            return Color.FromArgb(
                NewColor.A,
                CheckBounds(color.R + ErrorR * part / total),
                CheckBounds(color.G + ErrorG * part / total),
                CheckBounds(color.B + ErrorB * part / total)
            );
        }

        private byte CheckBounds(int value)
        {
            return (byte) (value < 0 ? 0 : (value > 255 ? 255 : value));
        }
    }
}