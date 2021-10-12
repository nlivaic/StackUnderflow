using System;

namespace StackUnderflow.WorkerServices.PointServices.Sorting
{
    public class InvalidPropertyMappingException : Exception
    {
        public InvalidPropertyMappingException(string message) : base(message)
        {
        }
    }
}
