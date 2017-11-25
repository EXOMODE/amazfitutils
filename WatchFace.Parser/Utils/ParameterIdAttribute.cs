using System;

namespace WatchFace.Parser.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ParameterIdAttribute : Attribute
    {
        public ParameterIdAttribute(byte id)
        {
            Id = id;
        }

        public byte Id { get; }
    }
}