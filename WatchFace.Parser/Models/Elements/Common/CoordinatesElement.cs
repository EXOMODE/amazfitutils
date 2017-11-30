namespace WatchFace.Parser.Models.Elements
{
    public class CoordinatesElement : CompositeElement
    {
        public CoordinatesElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public long X { get; set; }
        public long Y { get; set; }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    X = parameter.Value;
                    return new ValueElement(parameter, this, nameof(X));
                case 2:
                    Y = parameter.Value;
                    return new ValueElement(parameter, this, nameof(Y));
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}