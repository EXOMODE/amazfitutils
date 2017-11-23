using System;
using WatchFace.Models;

namespace WatchFace
{
    public class DuplicateParameterException : Exception
    {
        public DuplicateParameterException(Parameter paramter, string path) : base(
            $"Parameter with Id: {paramter.Id} was already set in this position: {path}."
        ) { }
    }
}