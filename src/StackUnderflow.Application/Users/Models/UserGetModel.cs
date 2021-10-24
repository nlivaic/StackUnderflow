using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Application.Users.Models
{
    public class UserGetModel
    {
        public Guid Id { get; set; }
        public string Email { get; private set; }
        public Uri WebsiteUrl { get; private set; }
        public string AboutMe { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public TimeSpan LastSeenBefore { get; private set; }
        public IEnumerable<string> Roles { get; set; }

        public class UserGetModelProfile : Profile
        {
            public UserGetModelProfile()
            {
                CreateMap<User, UserGetModel>()
                    .ForMember(
                        dest => dest.LastSeenBefore,
                        opts => opts.MapFrom(
                            src => DateTime.UtcNow - src.LastSeen))
                    .ForMember(
                        dest => dest.Roles,
                        opts => opts.MapFrom(
                            src => src.Roles.Select(r => r.Role.ToString())));
            }
        }
    }
}
