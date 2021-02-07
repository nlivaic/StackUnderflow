using System;
using System.Linq;
using System.Security.Claims;
using StackUnderflow.Api.Models;

namespace StackUnderflow.Api.Helpers
{
    public static class IdentityExtensions
    {
        public static string Username(this ClaimsPrincipal user) =>
            user.Claims.SingleOrDefault(c => c.Type == "nickname").Value;

        public static Guid UserId(this ClaimsPrincipal user) =>
            new Guid(user.Claims.SingleOrDefault(c => c.Type == "sub").Value);

        public static bool IsOwner(this ClaimsPrincipal user, IOwneable owneable) =>
            user.Identity.IsAuthenticated ? user.UserId() == owneable.UserId : false;
    }
}
