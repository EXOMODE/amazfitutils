using System.Collections.Generic;
using WatchFace.Parser.Models;

namespace WatchFace.Parser.Utils
{
    public class DrawingOrderIterator
    {
        public static IEnumerable<DrawingOrderPosition> Iterate(long drawingOrder)
        {
            var order = drawingOrder;
            int i = 0;

            while (order != 0)
            {
                if (i == 2)
                {
                    yield return DrawingOrderPosition.Delimiter;
                    ++i;
                    continue;
                }

                var position = (DrawingOrderPosition) ((order & 0xf000) >> 12);
                yield return position;
                order = (order << 4) & 0xffff;
                ++i;
            }
        }
    }
}