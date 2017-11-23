using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;
using WatchFace.Models;

namespace WatchFace.Utils
{
    public class ParametersConverter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static List<Parameter> Build<T>(T serializable)
        {
            var result = new List<Parameter>();
            foreach (var kv in SortedPropertiesDictionary<T>())
            {
                var id = kv.Key;
                var propertyInfo = kv.Value;
                var propertyType = propertyInfo.PropertyType;
                dynamic propertyValue = propertyInfo.GetValue(serializable, null);

                if (propertyType == typeof(long))
                    result.Add(new Parameter(id, (long) propertyValue));
                else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
                    throw new NotImplementedException();
                else
                    result.Add(new Parameter(id, Build(propertyValue)));
            }
            return result;
        }

        public static T Parse<T>(List<Parameter> descriptor, string path = "") where T : new()
        {
            var properties = SortedPropertiesDictionary<T>();

            var result = new T();
            Logger.Trace("Reading {0} {1}", typeof(T).Name, path);
            foreach (var parameter in descriptor)
            {
                var currentPath = string.Concat(path, '.', parameter.Id.ToString());
                var propertyInfo = properties[parameter.Id];
                if (propertyInfo == null)
                    throw new InvalidParameterException(parameter, path);

                var propertyType = propertyInfo.PropertyType;

                if (propertyType == typeof(long))
                {
                    dynamic propertyValue = propertyInfo.GetValue(result);
                    if (propertyValue != 0)
                        throw new DuplicateParameterException(parameter, path);

                    propertyInfo.SetValue(result, parameter.Value);
                }
                else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    dynamic propertyValue = propertyInfo.GetValue(result);
                    if (propertyValue == null)
                    {
                        propertyValue = Activator.CreateInstance(propertyType);
                        propertyInfo.SetValue(result, propertyValue);
                    }

                    try
                    {
                        var method = typeof(ParametersConverter).GetMethod(nameof(Parse));
                        var itemType = propertyType.GetGenericArguments()[0];
                        var generic = method.MakeGenericMethod(itemType);
                        dynamic parsedValue = generic.Invoke(null, new dynamic[] {parameter.Children, currentPath});
                        propertyValue.Add(parsedValue);
                    }
                    catch (TargetInvocationException e)
                    {
                        throw e.InnerException;
                    }
                }
                else
                {
                    dynamic propertyValue = propertyInfo.GetValue(result);
                    if (propertyValue != null)
                        throw new DuplicateParameterException(parameter, path);

                    try
                    {
                        var method = typeof(ParametersConverter).GetMethod(nameof(Parse));
                        var generic = method.MakeGenericMethod(propertyType);
                        dynamic parsedValue = generic.Invoke(null, new dynamic[] {parameter.Children, currentPath});
                        propertyInfo.SetValue(result, parsedValue);
                    }
                    catch (TargetInvocationException e)
                    {
                        throw e.InnerException;
                    }
                }
            }
            return result;
        }

        private static Dictionary<byte, PropertyInfo> SortedPropertiesDictionary<T>()
        {
            var typeInfo = typeof(T).GetTypeInfo();
            var properties = new Dictionary<byte, PropertyInfo>();
            foreach (var propertyInfo in typeInfo.DeclaredProperties)
            {
                var rawParameterAttribute =
                    (RawParameterAttribute) propertyInfo.GetCustomAttribute(typeof(RawParameterAttribute));
                if (rawParameterAttribute == null)
                    throw new ArgumentException(
                        $"Class {typeInfo.Name} doesn't have RawParameterAttribute on property {propertyInfo.Name}"
                    );
                if (properties.ContainsKey(rawParameterAttribute.Id))
                    throw new ArgumentException(
                        $"Class {typeInfo.Name} already has RawParameterAttribute with Id {rawParameterAttribute.Id}"
                    );

                properties[rawParameterAttribute.Id] = propertyInfo;
            }
            return properties.OrderBy(kv => kv.Key).ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }
}