using System;
using System.Collections.Generic;

namespace StackUnderflow.Api.Helpers
{
    public class ScopeInformation : IScopeInformation
    {
        public ScopeInformation()
        {
            Host = new Dictionary<string, string>
            {
                { "MachineName", Environment.MachineName }
            };
        }

        public Dictionary<string, string> Host { get; }
    }
}
