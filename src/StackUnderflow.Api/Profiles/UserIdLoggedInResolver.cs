using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using StackUnderflow.Api.Helpers;

namespace StackUnderflow.Api.Profiles
{
    public class UserIdLoggedInResolver<TSource, TDestination>
        : IValueResolver<TSource, TDestination, Guid>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserIdLoggedInResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid Resolve(TSource source, TDestination destination, Guid destMember, ResolutionContext context) =>
            _httpContextAccessor.HttpContext.User.UserId().Value;
    }
}