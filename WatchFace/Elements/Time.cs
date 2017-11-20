namespace WatchFace.Elements
{
    public class Time
    {
        private Switch _amPm;
        private TwoDigits _hours;
        private TwoDigits _minutes;
        private TwoDigits _seconds;

        public Time(TwoDigits hours, TwoDigits minutes, TwoDigits seconds, Switch amPm)
        {
            _hours = hours;
            _minutes = minutes;
            _seconds = seconds;
            _amPm = amPm;
        }
    }
}