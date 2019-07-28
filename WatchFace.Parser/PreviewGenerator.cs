using System.Collections.Generic;
using System.Drawing;
using WatchFace.Parser.Interfaces;
using WatchFace.Parser.Models;

namespace WatchFace.Parser
{
    public class PreviewGenerator
    {
        public static IEnumerable<Image> CreateAnimation(List<Parameter> descriptor, Bitmap[] images, IEnumerable<WatchState> states, Size size)
        {
            Models.Elements.WatchFace previewWatchFace = new Models.Elements.WatchFace(descriptor);

            foreach (var watchState in states)
            {
                using (var image = CreateFrame(previewWatchFace, images, watchState, size))
                {
                    yield return image;
                }
            }
        }

        public static IEnumerable<Image> CreateAnimation(List<Parameter> descriptor, Bitmap[] images, IEnumerable<WatchState> states) => CreateAnimation(descriptor, images, states, new Size(176, 176));

        public static Image CreateImage(IEnumerable<Parameter> descriptor, Bitmap[] images, WatchState state, Size size)
        {
            Models.Elements.WatchFace previewWatchFace = new Models.Elements.WatchFace(descriptor);

            return CreateFrame(previewWatchFace, images, state, size);
        }

        public static Image CreateImage(IEnumerable<Parameter> descriptor, Bitmap[] images, WatchState state) => CreateImage(descriptor, images, state, new Size(176, 176));

        private static Image CreateFrame(IDrawable watchFace, Bitmap[] resources, WatchState state, Size size)
        {
            var preview = new Bitmap(size.Width, size.Height, format: System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(preview);
            watchFace.Draw(graphics, resources, state);
            return preview;
        }

        private static Image CreateFrame(IDrawable watchFace, Bitmap[] resources, WatchState state) => CreateFrame(watchFace, resources, state, new Size(176, 176));
    }
}