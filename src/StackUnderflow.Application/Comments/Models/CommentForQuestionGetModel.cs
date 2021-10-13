using AutoMapper;
using StackUnderflow.Core.Entities;
using System;

namespace StackUnderflow.Application.Comments.Models
{
    public class CommentForQuestionGetModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public int VotesSum { get; set; }
        public bool IsOwner { get; set; }
        public bool IsModerator { get; set; }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Comment, CommentForQuestionGetModel>()
                    .ForMember(dest => dest.Username,
                        opts => opts.MapFrom(src => src.User.Username));
            }
        }
    }
}
