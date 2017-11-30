using System.Drawing;
using WatchFace.Parser.Models;

namespace WatchFace.Parser.Interfaces
{
    public interface IDrawable
    {
        void Draw(Graphics drawer, Bitmap[] resources, WatchState state);
    }
}