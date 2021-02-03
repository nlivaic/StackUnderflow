using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using StackUnderflow.Api.Models;

namespace StackUnderflow.Api.Helpers
{
    public static class IdentityExtensions
    {
        public static string Nickname(this IEnumerable<Claim> claims) =>
            claims.SingleOrDefault(c => c.Type == "nickname").Value;

        public static Guid UserId(this IEnumerable<Claim> claims) =>
            new Guid(claims.SingleOrDefault(c => c.Type == "sub").Value);

        public static bool IsOwner(this ClaimsPrincipal user, IOwneable owneable) =>
            user.Identity.IsAuthenticated ? user.Claims.UserId() == owneable.UserId : false;
    }
}
