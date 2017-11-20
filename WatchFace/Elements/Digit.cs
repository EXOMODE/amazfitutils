namespace WatchFace.Elements
{
    public class Digit
    {
        private int _imageIndex;
        private int _imagesCount;
        private int _x;
        private int _y;

        public Digit(int x, int y, int imageIndex, int imagesCount)
        {
            _x = x;
            _y = y;
            _imageIndex = imageIndex;
            _imagesCount = imagesCount;
        }
    }
}