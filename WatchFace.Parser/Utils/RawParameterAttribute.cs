using System;

namespace WatchFace.Parser.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RawParameterAttribute : Attribute
    {
        public byte Id { get; set; }
    }
}