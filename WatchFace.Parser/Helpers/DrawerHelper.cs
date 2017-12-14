using System.Collections.Generic;
using System.Drawing;
using WatchFace.Parser.Models;

namespace WatchFace.Parser.Helpers
{
    public class DrawerHelper
    {
        public static Size CalculateBounds(IEnumerable<Bitmap> images, long spacing)
        {
            long width = 0;
            var height = 0;

            foreach (var image in images)
            {
                width += image.Width + spacing;
                if (image.Height > height) height = image.Height;
            }

            width -= spacing;
            return new Size((int) width, height);
        }

        public static void DrawImages(Graphics drawer, IEnumerable<Bitmap> images, long spacing,
            TextAlignment alignment, Rectangle box)
        {
            var bitmap = CalculateBounds(images, spacing);

            int x, y;
            if (alignment.HasFlag(TextAlignment.Left))
                x = box.X;
            else if (alignment.HasFlag(TextAlignment.Right))
                x = box.Right - bitmap.Width + 1;
            else
                x = (box.Left + box.Right - bitmap.Width) >> 1;

            if (alignment.HasFlag(TextAlignment.Top))
                y = box.Top;
            else if (alignment.HasFlag(TextAlignment.Bottom))
                y = box.Bottom - bitmap.Height + 1;
            else
                y = (box.Top + box.Bottom - bitmap.Height) >> 1;

            if (x < box.Left) x = box.Left;
            if (y < box.Top) y = box.Top;

            foreach (var image in images)
            {
                drawer.DrawImage(image, x, y);
                x += image.Width + (int) spacing;
            }
        }
    }
}