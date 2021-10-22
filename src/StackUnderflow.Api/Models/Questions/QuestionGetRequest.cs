using System;
using AutoMapper;
using StackUnderflow.Api.Profiles;
using StackUnderflow.Application.Questions.Commands;

namespace StackUnderflow.Api.Models.Questions
{
    public class QuestionGetRequest
    {
        public Guid QuestionId { get; set; }

        public class QuestionGetRequestProfile : Profile
        {
            public QuestionGetRequestProfile()
            {
                CreateMap<QuestionGetRequest, GetQuestionQuery>()
                    .ForMember(
                        dest => dest.CurrentUserId,
                        opts => opts.MapFrom<UserIdResolver<QuestionGetRequest, GetQuestionQuery>>());
            }
        }
    }
}
