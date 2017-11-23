using System;
using WatchFace.Parser.Models;

namespace WatchFace
{
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException(Parameter paramter, string path) : base(
            $"Parameter with Id: {paramter.Id} is not supported in this position: {path}."
        ) { }
    }
}