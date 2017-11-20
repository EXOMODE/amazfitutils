namespace WatchFace.Elements
{
    public class TwoDigits
    {
        private Digit _tens;
        private Digit _ones;

        public TwoDigits(Digit tens, Digit ones)
        {
            _tens = tens;
            _ones = ones;
        }
    }
}