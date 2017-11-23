using System;

namespace WatchFace.Parser.Models
{
    [Flags]
    public enum ParameterFlags
    {
        Unknown = 1,
        HasChildren = 2,
        Unknown2 = 4
    }
}