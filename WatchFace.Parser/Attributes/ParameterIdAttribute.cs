using System;

namespace WatchFace.Parser.Attributes
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