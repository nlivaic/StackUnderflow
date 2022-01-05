using AutoMapper;
using StackUnderflow.Application.Answers.Models;

namespace StackUnderflow.Api.Models
{
    public class AnswerUpdateRequest
    {
        public string Body { get; set; }

        public class AnswerProfile : Profile
        {
            public AnswerProfile()
            {
                CreateMap<AnswerUpdateRequest, AnswerEditModel>()
                    .ForMember(
                        dest => dest.QuestionId,
                        opts => opts.Ignore())
                    .ForMember(
                        dest => dest.UserId,
                        opts => opts.Ignore())
                    .ForMember(
                        dest => dest.AnswerId,
                        opts => opts.Ignore());
            }
        }
    }
}
