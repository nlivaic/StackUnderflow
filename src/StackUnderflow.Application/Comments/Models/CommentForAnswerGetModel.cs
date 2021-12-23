using System;
using AutoMapper;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Application.Comments.Models
{
    public class CommentForAnswerGetModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; set; }
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
                CreateMap<Comment, CommentForAnswerGetModel>()
                    .ForMember(
                        dest => dest.AnswerId,
                        opts => opts.MapFrom(src => src.ParentAnswerId))
                    .ForMember(
                        dest => dest.QuestionId,
                        opts => opts.MapFrom(src => src.ParentAnswer.QuestionId))
                    .ForMember(
                        dest => dest.Username,
                        opts => opts.MapFrom(src => src.User.Username))
                    .ForMember(
                        dest => dest.VotesSum,
                        opts => opts.Ignore())
                    .ForMember(
                        dest => dest.IsOwner,
                        opts => opts.Ignore())
                    .ForMember(
                        dest => dest.IsModerator,
                        opts => opts.Ignore());
            }
        }
    }
}
