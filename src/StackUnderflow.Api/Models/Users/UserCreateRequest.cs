using System;
using AutoMapper;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Models
{
    public class UserCreateRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }

        class UserCreateRequestProfile : Profile
        {
            public UserCreateRequestProfile()
            {
                CreateMap<UserCreateRequest, UserCreateModel>();
            }
        }
    }
}
