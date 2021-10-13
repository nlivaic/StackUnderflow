using AutoMapper;
using StackUnderflow.Application.Questions.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackUnderflow.Api.Models
{
    public class QuestionSummaryGetViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string CreatedOn { get; set; }
        public bool HasAcceptedAnswer { get; set; }
        public IEnumerable<string> Tags { get; set; } = new List<string>();
        public int Answers { get; set; }
        public int VotesSum { get; set; }

        public class QuestionProfile : Profile
        {
            public QuestionProfile()
            {
                CreateMap<QuestionSummaryGetModel, QuestionSummaryGetViewModel>()
                    .ForMember(dest => dest.Tags,
                        opts => opts.MapFrom(src => src.Tags.Select(t => t.Name)))
                    .ForMember(dest => dest.CreatedOn,
                        opts => opts.MapFrom(src => src.CreatedOn.ToString("yyyy-MM-dd hh:mm:ss")));
            }
        }
    }
}
