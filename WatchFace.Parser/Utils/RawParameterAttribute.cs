using System;

namespace WatchFace.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RawParameterAttribute : Attribute
    {
        public byte Id { get; set; }
    }
}