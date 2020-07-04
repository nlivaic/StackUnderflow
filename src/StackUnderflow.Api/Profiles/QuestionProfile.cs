using AutoMapper;
using StackUnderflow.Api.Models;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<QuestionWithUserAndAllCommentsModel, QuestionWithUserAndAllCommentsViewModel>();
        }
    }
}