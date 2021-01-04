using System;

namespace StackUnderflow.Api.Exceptions
{
    public class InvalidPropertyMappingException : Exception
    {
        public InvalidPropertyMappingException(string message) : base(message)
        {
        }
    }
}
