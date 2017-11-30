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
            if (state.CurrentWeather != WeatherCondition.Unknown)
                drawer.DrawImage(LoadWeatherImage(state.CurrentWeather), Current.X, Current.Y);
            else if (state.TodayWeather != WeatherCondition.Unknown)
                drawer.DrawImage(LoadWeatherImage(state.TodayWeather), Today.X, Today.Y);
            else if (state.TomorrowWeather != WeatherCondition.Unknown)
                drawer.DrawImage(LoadWeatherImage(state.TomorrowWeather), Tomorrow.X, Tomorrow.Y);
        }

        private static Bitmap LoadWeatherImage(WeatherCondition weather)
        {
            var appLocation = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return (Bitmap) Image.FromFile(System.IO.Path.Combine(appLocation, "WeatherIcons", $"{(int) weather}.png"));
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