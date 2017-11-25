using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using NLog;

namespace WatchFace.Parser.Utils
{
    public class ImagesLoader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _imagesDirectory;

        private Dictionary<long, long> _mapping;

        public ImagesLoader(string imagesDirectory)
        {
            Images = new List<Bitmap>();
            _mapping = new Dictionary<long, long>();
            _imagesDirectory = imagesDirectory;
        }

        public List<Bitmap> Images { get; }

        public void Process<T>(T serializable, string path = "")
        {
            if (!string.IsNullOrEmpty(path)) Logger.Trace("Loading images for {0} '{1}'", path, typeof(T).Name);

            long? lastImageIndexValue = null;

            foreach (var kv in SortedPropertiesDictionary<T>())
            {
                var id = kv.Key;
                var currentPath = string.IsNullOrEmpty(path)
                    ? id.ToString()
                    : string.Concat(path, '.', id.ToString());

                var propertyInfo = kv.Value;
                var propertyType = propertyInfo.PropertyType;
                dynamic propertyValue = propertyInfo.GetValue(serializable, null);

                var imageIndexAttribute = (ParameterImageIndexAttribute) propertyInfo.GetCustomAttribute(
                    typeof(ParameterImageIndexAttribute)
                );
                var imagesCountAttribute = (ParameterImagesCountAttribute) propertyInfo.GetCustomAttribute(
                    typeof(ParameterImagesCountAttribute)
                );

                if (imagesCountAttribute != null && imageIndexAttribute != null)
                    throw new ArgumentException(
                        $"Property {propertyInfo.Name} can't have both ParameterImageIndexAttribute and ParameterImagesCountAttribute"
                    );

                if (propertyType == typeof(long))
                {
                    if (imageIndexAttribute != null)
                    {
                        lastImageIndexValue = propertyValue;
                        var mappedIndex = LoadImage(propertyValue);
                        propertyInfo.SetValue(serializable, mappedIndex);
                    }
                    else if (imagesCountAttribute != null)
                    {
                        if (lastImageIndexValue == null)
                            throw new ArgumentException(
                                $"Property {propertyInfo.Name} can't be processed becuase ImageIndex isn't present or it is zero"
                            );
                        for (var i = lastImageIndexValue + 1; i < lastImageIndexValue + propertyValue; i++)
                            LoadImage(i.Value);
                    }
                }
                else
                {
                    if (imagesCountAttribute == null && imageIndexAttribute == null)
                    {
                        if (propertyValue != null)
                            Process(propertyValue, currentPath);
                    }
                    else
                    {
                        throw new ArgumentException(
                            $"Property {propertyInfo.Name} with type {propertyType.Name} can't have ParameterImageIndexAttribute or ParameterImagesCountAttribute"
                        );
                    }
                }
            }
        }

        private long LoadImage(long index)
        {
            if (_mapping.ContainsKey(index))
                return _mapping[index];

            var fileName = Path.Combine(_imagesDirectory, $"{index}.png");
            var newImageIndex = Images.Count;
            Images.Add((Bitmap) Image.FromFile(fileName));
            _mapping[index] = newImageIndex;
            return newImageIndex;
        }

        private static Dictionary<byte, PropertyInfo> SortedPropertiesDictionary<T>()
        {
            var typeInfo = typeof(T).GetTypeInfo();
            var properties = new Dictionary<byte, PropertyInfo>();
            foreach (var propertyInfo in typeInfo.DeclaredProperties)
            {
                var parameterIdAttribute =
                    (ParameterIdAttribute) propertyInfo.GetCustomAttribute(typeof(ParameterIdAttribute));
                if (parameterIdAttribute == null)
                    throw new ArgumentException(
                        $"Class {typeInfo.Name} doesn't have ParameterIdAttribute on property {propertyInfo.Name}"
                    );
                if (properties.ContainsKey(parameterIdAttribute.Id))
                    throw new ArgumentException(
                        $"Class {typeInfo.Name} already has ParameterIdAttribute with Id {parameterIdAttribute.Id}"
                    );

                properties[parameterIdAttribute.Id] = propertyInfo;
            }
            return properties.OrderBy(kv => kv.Key).ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }
}