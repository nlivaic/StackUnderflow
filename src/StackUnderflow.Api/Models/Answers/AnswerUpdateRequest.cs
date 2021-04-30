using AutoMapper;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Models
{
    public class AnswerUpdateRequest
    {
        public string Body { get; set; }

        public class AnswerProfile : Profile
        {
            public AnswerProfile()
            {
                CreateMap<AnswerUpdateRequest, AnswerEditModel>();
            }
        }

    }
}
