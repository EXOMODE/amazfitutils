namespace WatchFace.Parser.Models.Elements
{
    public class Element
    {
        protected readonly string _name;
        protected readonly Element _parent;

        protected Element() { }

        protected Element(Parameter parameter, Element parent, string name = null)
        {
            Id = parameter.Id;
            _name = name;
            _parent = parent;
        }

        protected bool HasParent => _parent != null;
        protected string StringId => Id.ToString();

        public byte? Id { get; }
        public string Name => _name ?? $"Unknown{Id}";
        public string Path => HasParent ? $"{_parent.Path}.{StringId}" : StringId;
    }
}