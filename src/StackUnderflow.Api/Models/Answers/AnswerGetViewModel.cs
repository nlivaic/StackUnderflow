using AutoMapper;
using StackUnderflow.Core.Models;
using System;

namespace StackUnderflow.Api.Models
{
    public class AnswerGetViewModel : IOwneable
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Body { get; set; }
        public bool IsAcceptedAnswer { get; set; }
        public string CreatedOn { get; set; }
        public int VotesSum { get; set; }
        public bool IsOwner { get; set; }
        public bool IsModerator { get; set; }


        public class AnswerProfile : Profile
        {
            public AnswerProfile()
            {
                CreateMap<AnswerGetModel, AnswerGetViewModel>()
                    .ForMember(
                        dest => dest.CreatedOn,
                        opts => opts.MapFrom(
                            src => src.CreatedOn.ToString("yyyy-MM-dd hh:mm:ss")));
            }
        }
    }
}
