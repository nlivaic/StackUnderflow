using System;
using System.Collections.Generic;
using System.Reflection;

namespace StackUnderflow.Api.Helpers
{
    public class ScopeInformation : IScopeInformation
    {
        public ScopeInformation()
        {
            Host = new Dictionary<string, string>
            {
                { "MachineName", Environment.MachineName },
                { "Entrypoint", Assembly.GetExecutingAssembly().GetName().Name }
            };
        }

        public Dictionary<string, string> Host { get; }
    }
}
