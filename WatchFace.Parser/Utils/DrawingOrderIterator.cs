using System.Collections.Generic;
using WatchFace.Parser.Models;

namespace WatchFace.Parser.Utils
{
    public class DrawingOrderIterator
    {
        public static IEnumerable<DrawingOrderPosition> Iterate(long drawingOrder)
        {
            var order = drawingOrder;
            while (order != 0)
            {
                var position = (DrawingOrderPosition) ((order & 0xf000) >> 12);
                yield return position;
                order = (order << 4) & 0xffff;
            }
        }
    }
}