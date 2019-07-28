namespace WatchFace.Parser.Models.Elements
{
    public class CoordinatesElement : CompositeElement
    {
        public CoordinatesElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public long X { get; set; }
        public long Y { get; set; }
        public long X2 { get; set; }
        public long Y2 { get; set; }
        public long X3 { get; set; }

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
                case 3:
                    X2 = parameter.Value;
                    return new ValueElement(parameter, this, nameof(X2));
                case 4:
                    Y2 = parameter.Value;
                    return new ValueElement(parameter, this, nameof(Y2));
                case 5:
                    X3 = parameter.Value;
                    return new ValueElement(parameter, this, nameof(X3));
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}