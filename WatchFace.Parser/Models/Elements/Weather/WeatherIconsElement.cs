using System.Drawing;
using System.Reflection;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class WeatherIconsElement : CompositeElement, IDrawable
    {
        public WeatherIconsElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public CoordinatesElement Current { get; set; }
        public ImageSetElement CustomIcon { get; set; }
        public CoordinatesElement CurrentAlt { get; set; }
        public CoordinatesElement Unknown4 { get; set; }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            var useAltCoordinates = CurrentAlt != null && state.CurrentTemperature == null;
            var iconCoordinates = useAltCoordinates ? CurrentAlt : Current;

            if (state.CurrentWeather > WeatherCondition.VeryHeavyDownpour ||
                state.CurrentWeather < WeatherCondition.Unknown) return;

            if (iconCoordinates != null)
                drawer.DrawImage(LoadWeatherImage(state.CurrentWeather), iconCoordinates.X, iconCoordinates.Y);

            if (CustomIcon != null)
                drawer.DrawImage(resources[CustomIcon.ImageIndex + (int)state.CurrentWeather], CustomIcon.X, CustomIcon.Y);
        }

        private static Bitmap LoadWeatherImage(WeatherCondition weather)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var imageStream = assembly.GetManifestResourceStream($"WatchFace.Parser.WeatherIcons.{(int) weather}.png");
            return (Bitmap) Image.FromStream(imageStream);
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Current = new CoordinatesElement(parameter, this);
                    return Current;
                case 2:
                    CustomIcon = new ImageSetElement(parameter, this);
                    return CustomIcon;
                case 3:
                    CurrentAlt = new CoordinatesElement(parameter, this);
                    return CurrentAlt;
                case 4:
                    Unknown4 = new CoordinatesElement(parameter, this);
                    return Unknown4;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}