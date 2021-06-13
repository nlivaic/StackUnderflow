using System;

namespace StackUnderflow.Application.Services.Sorting
{
    public class InvalidPropertyMappingException : Exception
    {
        public InvalidPropertyMappingException(string message) : base(message)
        {
        }
    }
}
