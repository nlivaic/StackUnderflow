using AutoMapper;
using StackUnderflow.Api.Models;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<CommentGetModel, CommentGetViewModel>();
            CreateMap<CommentOnQuestionCreateRequest, CommentOnQuestionCreateModel>();
        }
    }
}
