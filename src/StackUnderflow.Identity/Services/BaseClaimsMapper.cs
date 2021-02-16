using System;
using System.Collections.Generic;

namespace StackUnderflow.Identity.Services
{
    public abstract class BaseClaimsMapper
    {
        public string this[string claimKey]
        {
            get
            {
                SourceClaims.TryGetValue(claimKey, out var mappedClaim);
                if (string.IsNullOrWhiteSpace(mappedClaim))
                {
                    throw new Exception($"Source claim unknown: {claimKey}.");
                }
                return mappedClaim;
            }
        }

        protected abstract Dictionary<string, string> SourceClaims { get; }
    }
}
