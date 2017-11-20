namespace WatchFace.Elements
{
    public class Image
    {
        private int _imageIndex;
        private int _x;
        private int _y;

        public Image(int x, int y, int imageIndex)
        {
            _x = x;
            _y = y;
            _imageIndex = imageIndex;
        }
    }
}