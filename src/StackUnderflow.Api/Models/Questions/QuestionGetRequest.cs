using AutoMapper;
using StackUnderflow.Api.Profiles;
using StackUnderflow.Core.Models.Questions;
using System;

namespace StackUnderflow.Api.Models.Questions
{
    public class QuestionGetRequest
    {
        public Guid Id { get; set; }

        public class QuestionGetRequestProfile : Profile
        {
            public QuestionGetRequestProfile()
            {
                CreateMap<QuestionGetRequest, QuestionFindModel>()
                    .ForMember(dest => dest.UserId,
                        opts => opts.MapFrom<UserIdResolver<QuestionGetRequest, QuestionFindModel>>());
            }
        }

    }
}
