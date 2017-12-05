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
        public CoordinatesElement Today { get; set; }
        public CoordinatesElement Tomorrow { get; set; }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            if (state.CurrentWeather != WeatherCondition.Unknown && Current != null)
                drawer.DrawImage(LoadWeatherImage(state.CurrentWeather), Current.X, Current.Y);
            else if (state.TodayWeather != WeatherCondition.Unknown && Today != null)
                drawer.DrawImage(LoadWeatherImage(state.TodayWeather), Today.X, Today.Y);
            else if (state.TomorrowWeather != WeatherCondition.Unknown && Tomorrow != null)
                drawer.DrawImage(LoadWeatherImage(state.TomorrowWeather), Tomorrow.X, Tomorrow.Y);
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
                case 3:
                    Today = new CoordinatesElement(parameter, this);
                    return Today;
                case 4:
                    Tomorrow = new CoordinatesElement(parameter, this);
                    return Tomorrow;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}