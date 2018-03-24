using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using BumpKit;
using WatchFace.Parser.Interfaces;
using WatchFace.Parser.Models;

namespace WatchFace.Parser
{
    public class PreviewGenerator
    {
        public static void CreateGif(List<Parameter> descriptor, Bitmap[] images, Stream outputStream)
        {
            var previewWatchFace = new Models.Elements.WatchFace(descriptor);
            var watchState = new WatchState();
            var time = watchState.Time;

            using (var encoder = new GifEncoder(outputStream))
            {
                for (var i = 0; i < 10; i++)
                {
                    var num = i + 1;
                    watchState.BatteryLevel = num * 10;

                    if (i < 2)
                        watchState.Pulse = null;
                    else
                        watchState.Pulse = 65 + num * 2;
                    watchState.Steps = num * 1000;
                    watchState.Calories = num * 75;
                    watchState.Distance = num * 700;

                    watchState.Bluetooth = num > 1 && num < 6;
                    watchState.Unlocked = num > 2 && num < 7;
                    watchState.Alarm = num > 3 && num < 8;
                    watchState.DoNotDisturb = num > 4 && num < 9;

                    watchState.DayTemperature += 2;
                    watchState.NightTemperature += 4;

                    if (num < 3)
                        watchState.CurrentTemperature = null;
                    else if (num == 3)
                        watchState.CurrentTemperature = -10;
                    else
                        watchState.CurrentTemperature += 6;

                    watchState.Time = new DateTime(time.Year, num, num * 2 + 5, i * 2, i * 6, i);
                    using (var image = CreateFrame(previewWatchFace, images, watchState))
                    {
                        encoder.AddFrame(image, frameDelay: TimeSpan.FromSeconds(1));
                    }
                }
            }
        }

        public static Image CreateImage(IEnumerable<Parameter> descriptor, Bitmap[] resources)
        {
            var previewWatchFace = new Models.Elements.WatchFace(descriptor);
            return CreateFrame(previewWatchFace, resources, new WatchState());
        }

        private static Image CreateFrame(IDrawable watchFace, Bitmap[] resources, WatchState state)
        {
            var background = resources[0];
            var preview = new Bitmap(background.Width, background.Height);
            var graphics = Graphics.FromImage(preview);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            watchFace.Draw(graphics, resources, state);
            return preview;
        }
    }
}