using AutoMapper;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentModel>()
                .ForMember(dest => dest.Username,
                opts => opts.MapFrom(src => src.User.Username));
        }
    }
}
