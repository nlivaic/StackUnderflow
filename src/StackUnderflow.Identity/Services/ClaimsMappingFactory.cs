using System;
using System.Collections.Generic;
using IdentityModel;

namespace StackUnderflow.Identity.Services
{
    public class ClaimsMappingFactory : IClaimsMappingFactory
    {
        public BaseClaimsMapper CreateMapper(string providerName)
        {
            switch (providerName)
            {
                case "Facebook":
                    {
                        return new FacebookClaimsMapper();
                    }
                default:
                    throw new Exception($"Unknown provider {providerName}.");
            }
        }
    }
}
