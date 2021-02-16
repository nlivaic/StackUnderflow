using System.Collections.Generic;
using IdentityModel;

namespace StackUnderflow.Identity.Services
{
    public class FacebookClaimsMapper : BaseClaimsMapper
    {
        protected override Dictionary<string, string> SourceClaims
        {
            get => new Dictionary<string, string>()
                {
                    { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", JwtClaimTypes.Email },
                    { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", JwtClaimTypes.Name },
                    { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", JwtClaimTypes.GivenName },
                    { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname", JwtClaimTypes.FamilyName }
                };
        }
    }
}
