using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using StackUnderflow.Api.Models;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Api.Helpers
{
    public static class IdentityExtensions
    {
        public static string Username(this ClaimsPrincipal user) =>
            user.Claims.SingleOrDefault(c => c.Type == "nickname").Value;

        public static Guid? UserId(this ClaimsPrincipal user)
        {
            var subClaim = user.Claims.SingleOrDefault(c => c.Type == "sub");
            return subClaim == null
                ? (Guid?)null
                : new Guid(subClaim.Value);
        }

        public static bool IsOwner(this ClaimsPrincipal user, IOwneable owneable) =>
            user.Identity.IsAuthenticated && user.UserId() == owneable.UserId;
    }
}
