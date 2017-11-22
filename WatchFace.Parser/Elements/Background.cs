using System.Collections.Generic;
using NLog;
using WatchFace.Elements.BasicElements;
using WatchFace.Models;

namespace WatchFace.Elements
{
    public class Background
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public Image Image { get; set; }

        public static Background Parse(List<Parameter> parameters, string path)
        {
            Logger.Trace("Reading {0}", path);
            var result = new Background();
            foreach (var parameter in parameters)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                switch (parameter.Id)
                {
                    case 1:
                        result.Image = Image.Parse(parameter.Children, currentPath);
                        break;
                    default:
                        throw new InvalidParameterException(parameter, path);
                }
            }
            return result;
        }
    }
}