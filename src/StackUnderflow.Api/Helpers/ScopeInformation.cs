using System;
using System.Collections.Generic;

namespace StackUnderflow.Api.Helpers
{
    /// <summary>
    /// Add additional entries to Host, e.g. EntryAssembly.
    /// </summary>
    public interface IScopeInformation
    {
        Dictionary<string, string> Host { get; }
    }

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
