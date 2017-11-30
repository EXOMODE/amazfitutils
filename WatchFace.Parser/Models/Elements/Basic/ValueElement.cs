namespace WatchFace.Parser.Models.Elements
{
    public class ValueElement : Element
    {
        public ValueElement(Parameter parameter, Element parent, string name = null) : base(parameter, parent, name)
        {
            Value = parameter.Value;
        }

        public long Value { get; set; }
    }
}