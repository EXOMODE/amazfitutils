using System.Collections.Generic;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class Background
    {
        public Image Image { get; set; }

        public static Background Parse(List<Parameter> parameters)
        {
            var result = new Background();
            foreach (var parameter in parameters)
                switch (parameter.Id)
                {
                    case 1:
                        result.Image = Image.Parse(parameter.Children);
                        break;
                    default:
                        throw new InvalidParameterException(parameter);
                }
            return result;
        }
    }
}