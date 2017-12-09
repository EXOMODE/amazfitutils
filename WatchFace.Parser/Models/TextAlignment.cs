using System;

namespace WatchFace.Parser.Models
{
    [Flags]
    public enum TextAlignment
    {
        Left = 2,
        Right = 4,
        HCenter = 8,

        Top = 16,
        Bottom = 32,
        VCenter = 64,

        TopCenter = Top | HCenter,
        TopLeft = Top | Left,
        TopRight = Top | Right,

        Center = VCenter | HCenter,
        CenterLeft = VCenter | Left,
        CenterRight = VCenter | Right,

        BottomCenter = Bottom | HCenter,
        BottomLeft = Bottom | Left,
        BottomRight = Bottom | Right
    }
}