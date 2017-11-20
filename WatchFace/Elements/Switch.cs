namespace WatchFace.Elements
{
    public class Switch
    {
        private int _imageIndexOff;
        private int _imageIndexOn;
        private int _x;
        private int _y;

        public Switch(int x, int y, int imageIndexOff, int imageIndexOn)
        {
            _x = x;
            _y = y;
            _imageIndexOff = imageIndexOff;
            _imageIndexOn = imageIndexOn;
        }
    }
}