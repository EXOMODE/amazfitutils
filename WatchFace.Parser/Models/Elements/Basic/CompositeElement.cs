using System.Collections.Generic;

namespace WatchFace.Parser.Models.Elements
{
    public class CompositeElement : Element
    {
        public CompositeElement() { }

        public CompositeElement(IEnumerable<Parameter> parameters) => CreateChilds(parameters);

        public CompositeElement(Parameter parameter, Element parent, string name = null) : base(parameter, parent, name) => CreateChilds(parameter.Children);

        public List<Element> Children { get; } = new List<Element>();

        public void CreateChilds(IEnumerable<Parameter> parameter)
        {
            if (parameter == null) return;

            foreach (var parameterChild in parameter) Children.Add(CreateChildForParameter(parameterChild));
        }

        protected virtual Element CreateChildForParameter(Parameter parameter)
        {
            if (parameter.HasChildren) return new ContainerElement(parameter, this);

            return new ValueElement(parameter, this);
        }
    }
}