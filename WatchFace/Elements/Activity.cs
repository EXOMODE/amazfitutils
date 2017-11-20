namespace WatchFace.Elements
{
    public class Activity
    {
        private Number _calories;
        private Number _pulse;
        private Number _steps;

        public Activity(Number steps, Number calories, Number pulse)
        {
            _steps = steps;
            _calories = calories;
            _pulse = pulse;
        }
    }
}