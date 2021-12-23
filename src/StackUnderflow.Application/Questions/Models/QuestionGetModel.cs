using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using StackUnderflow.Application.Tags.Models;
using StackUnderflow.Application.Votes.Models;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Application.Questions.Models
{
    public class QuestionGetModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool HasAcceptedAnswer { get; set; }
        public DateTime CreatedOn { get; set; }
        public int VotesSum { get; set; }
        public IEnumerable<VoteGetModel> Votes { get; set; } = new List<VoteGetModel>();
        public IEnumerable<TagGetModel> Tags { get; set; } = new List<TagGetModel>();
        public bool IsOwner { get; set; }
        public bool IsModerator { get; set; }

        public class QuestionGetModelProfile : Profile
        {
            public QuestionGetModelProfile()
            {
                CreateMap<Question, QuestionGetModel>()
                    .ForMember(
                        dest => dest.Username,
                        opts => opts.MapFrom(src => src.User.Username))
                    .ForMember(
                        dest => dest.Tags,
                        opts => opts.MapFrom(src => src.QuestionTags.Select(qt => qt.Tag)))
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
