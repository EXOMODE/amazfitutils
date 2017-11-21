using System;
using WatchFace.Models;

namespace WatchFace
{
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException(Parameter paramter) : base(
            $"Parameter with Id: {paramter.Id} is not supported in this position."
        ) { }
    }
}