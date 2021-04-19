using System;

namespace StackUnderflow.Common.Exceptions
{
    public class LimitNotMappable : Exception
    {
        public LimitNotMappable(string message)
            : base(message)
        {
        }
    }
}
