using System;

namespace WatchFace.Parser.Models
{
    public class WatchState
    {
        public DateTime Time { get; set; } = DateTime.Now;

        public int Steps { get; set; } = 14876;
        public int Goal { get; set; } = 8000;
        public int Calories { get; set; } = 764;
        public int Distance { get; set; } = 2367;
        public int? Pulse { get; set; } = 62;

        public int? CurrentTemperature { get; set; } = -10;
        public int? DayTemperature { get; set; } = -15;
        public int? NightTemperature { get; set; } = -24;
        public int? TomorrowDayTemperature { get; set; }
        public int? TomorrowNightTemperature { get; set; }
        public WeatherCondition CurrentWeather { get; set; } = WeatherCondition.Cloudy;
        public WeatherCondition TodayWeather { get; set; } = WeatherCondition.Unknown;
        public WeatherCondition TomorrowWeather { get; set; } = WeatherCondition.Unknown;
        public AirCondition Air { get; set; } = AirCondition.Excellent;

        public int BatteryLevel { get; set; } = 67;
        public bool Bluetooth { get; set; } = true;
        public bool Unlocked { get; set; } = true;
        public bool DoNotDisturb { get; set; }
        public bool Alarm { get; set; }
    }
}