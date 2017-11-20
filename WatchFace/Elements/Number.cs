namespace WatchFace.Elements
{
    public class Number
    {
        private int _bottomRightX;
        private int _bottomRightY;
        private int _imageIndex;
        private int _imagesCount;
        private int _topLeftX;
        private int _topLeftY;
        private int _unknown1;
        private int _unknown2;

        public Number(int topLeftX, int topLeftY, int bottomRightX, int bottomRightY, int unknown1, int unknown2,
            int imageIndex, int imagesCount)
        {
            _topLeftX = topLeftX;
            _topLeftY = topLeftY;
            _bottomRightX = bottomRightX;
            _bottomRightY = bottomRightY;
            _unknown1 = unknown1;
            _unknown2 = unknown2;
            _imageIndex = imageIndex;
            _imagesCount = imagesCount;
        }
    }
}