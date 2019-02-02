using System;
using System.Collections.Generic;
using System.Reflection;
using NLog;
using WatchFace.Parser.Models;

namespace WatchFace.Parser.Utils
{
    public class ParametersConverter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static List<Parameter> Build<T>(T serializable, string path = "")
        {
            var result = new List<Parameter>();
            var currentType = typeof(T);

            foreach (var kv in ElementsHelper.SortedProperties<T>())
            {
                var id = kv.Key;
                var currentPath = string.IsNullOrEmpty(path)
                    ? id.ToString()
                    : string.Concat(path, '.', id.ToString());

                var propertyInfo = kv.Value;
                var propertyType = propertyInfo.PropertyType;
                dynamic propertyValue = propertyInfo.GetValue(serializable, null);

                if (propertyValue == null)
                    continue;

                if (propertyType == typeof(long) ||
                    propertyType == typeof(TextAlignment) ||
                    propertyType == typeof(bool))
                {
                    long value;
                    if (propertyType == typeof(bool))
                        value = propertyValue ? 1 : 0;
                    else
                        value = (long) propertyValue;

                    Logger.Trace("{0} '{1}': {2}", currentPath, propertyInfo.Name, value);
                    result.Add(new Parameter(id, value));
                }
                else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Logger.Trace("{0} '{1}': {2}", currentPath, propertyInfo.Name, propertyValue);
                    result.Add(new Parameter(id, propertyValue));
                }
                else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    foreach (var item in propertyValue)
                    {
                        Logger.Trace("{0} '{1}'", currentPath, propertyInfo.Name);
                        result.Add(new Parameter(id, Build(item, currentPath)));
                    }
                }
                else
                {
                    var innerParameters = Build(propertyValue, currentPath);
                    if (innerParameters.Count > 0)
                    {
                        Logger.Trace("{0} '{1}'", currentPath, propertyInfo.Name);
                        result.Add(new Parameter(id, innerParameters));
                    }
                    else
                        Logger.Trace("{0} '{1}': Skipped because of empty", currentPath, propertyInfo.Name);
                }
            }

            return result;
        }

        public static T Parse<T>(List<Parameter> descriptor, string path = "") where T : new()
        {
            var properties = ElementsHelper.SortedProperties<T>();
            var result = new T();
            var currentType = typeof(T);

            var thisMethod = typeof(ParametersConverter).GetMethod(nameof(Parse));

            foreach (var parameter in descriptor)
            {
                var currentPath = string.IsNullOrEmpty(path)
                    ? parameter.Id.ToString()
                    : string.Concat(path, '.', parameter.Id.ToString());
                if (!properties.ContainsKey(parameter.Id))
                    throw new ArgumentException($"Parameter {parameter.Id} isn't supported for {currentType.Name}");

                var propertyInfo = properties[parameter.Id];
                var propertyType = propertyInfo.PropertyType;

                if (propertyType == typeof(long) ||
                    propertyType == typeof(TextAlignment) ||
                    propertyType == typeof(bool) ||
                    propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Logger.Trace("{0} '{1}': {2}", currentPath, propertyInfo.Name, parameter.Value);
                    dynamic propertyValue = propertyInfo.GetValue(result, null);

                    if (propertyType.IsGenericType && propertyValue != null)
                        throw new ArgumentException($"Parameter {parameter.Id} is already set for {currentType.Name}");

                    if (!propertyType.IsGenericType && propertyType == typeof(long) && propertyValue != 0)
                        throw new ArgumentException($"Parameter {parameter.Id} is already set for {currentType.Name}");

                    if (propertyType == typeof(TextAlignment))
                        propertyInfo.SetValue(result, (TextAlignment) parameter.Value, null);
                    else if (propertyType == typeof(bool))
                        propertyInfo.SetValue(result, parameter.Value > 0, null);
                    else
                        propertyInfo.SetValue(result, parameter.Value, null);
                }
                else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Logger.Trace("{0} '{1}'", currentPath, propertyInfo.Name);
                    dynamic propertyValue = propertyInfo.GetValue(result, null);
                    if (propertyValue == null)
                    {
                        propertyValue = Activator.CreateInstance(propertyType);
                        propertyInfo.SetValue(result, propertyValue, null);
                    }

                    try
                    {
                        var generic = thisMethod.MakeGenericMethod(propertyType.GetGenericArguments()[0]);
                        dynamic parsedValue = generic.Invoke(null,
                            new dynamic[] {parameter.Children, currentPath});
                        propertyValue.Add(parsedValue);
                    }
                    catch (TargetInvocationException e)
                    {
                        throw e.InnerException;
                    }
                }
                else
                {
                    Logger.Trace("{0} '{1}'", currentPath, propertyInfo.Name);
                    dynamic propertyValue = propertyInfo.GetValue(result, null);
                    if (propertyValue != null)
                        throw new ArgumentException($"Parameter {parameter.Id} is already set for {currentType.Name}");

                    try
                    {
                        var generic = thisMethod.MakeGenericMethod(propertyType);
                        dynamic parsedValue = generic.Invoke(null, new object[] {parameter.Children, currentPath});
                        propertyInfo.SetValue(result, parsedValue, null);
                    }
                    catch (TargetInvocationException e)
                    {
                        throw e.InnerException;
                    }
                }
            }

            return result;
        }
    }
}