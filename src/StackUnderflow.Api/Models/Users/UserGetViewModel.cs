using System;
using System.Collections.Generic;
using AutoMapper;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Models
{
    public class UserGetViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; private set; }
        public Uri WebsiteUrl { get; private set; }
        public string AboutMe { get; private set; }
        public string CreatedOn { get; private set; }
        public string LastSeenBeforeDays { get; private set; }
        public IEnumerable<string> Roles { get; private set; }

        public class UserGetViewModelProfile : Profile
        {
            public UserGetViewModelProfile()
            {
                CreateMap<UserGetModel, UserGetViewModel>()
                    .ForMember(dest => dest.CreatedOn,
                        opts => opts.MapFrom(src => src.CreatedOn.ToString("yyyy-MM-dd")))
                    .ForMember(dest => dest.LastSeenBeforeDays,
                        opts => opts.MapFrom(
                            src => src.LastSeenBefore.Days));
            }
        }
    }
}
