using System.Collections.Generic;

namespace WatchFace.Models
{
    public class Resource
    {
        public Resource(byte id, List<Parameter> descriptor)
        {
            Id = id;
            Descriptor = descriptor;
        }

        public int Id { get; }
        public List<Parameter> Descriptor { get; }
    }
}