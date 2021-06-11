using AutoMapper;
using StackUnderflow.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackUnderflow.Core.Models
{
    public class QuestionSummaryGetModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ShortBody { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool HasAcceptedAnswer { get; set; }
        public IEnumerable<TagGetModel> Tags { get; set; } = new List<TagGetModel>();
        public int Answers { get; set; }
        public int VotesSum { get; set; }

        public class QuestionSummaryGetModelProfile : Profile
        {
            public QuestionSummaryGetModelProfile()
            {
                CreateMap<Question, QuestionSummaryGetModel>()
                    .ForMember(dest => dest.ShortBody,
                        opts => opts.MapFrom(src => src.Body.Substring(0, 50) + "..."))
                    .ForMember(dest => dest.Username,
                        opts => opts.MapFrom(src => src.User.Username))
                    .ForMember(dest => dest.Answers,
                        opts => opts.MapFrom(src => src.Answers.Count()))
                    .ForMember(dest => dest.Tags,
                        opts => opts.MapFrom(src => src.QuestionTags.Select(qt => qt.Tag)))
                    .ForMember(dest => dest.VotesSum,
                        opts => opts.MapFrom(src => src.Votes.Count()));    // Should be read from cache
            }
        }
    }
}
