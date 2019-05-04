using System;

namespace Resources
{
    public class InvalidResourceException : Exception
    {
        public InvalidResourceException(string message) : base(message) { }
    }
}