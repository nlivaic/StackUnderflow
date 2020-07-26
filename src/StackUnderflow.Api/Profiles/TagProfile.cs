using AutoMapper;
using StackUnderflow.Api.Models;
using StackUnderflow.Core.Models;

namespace Namespace
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<TagGetModel, TagGetViewModel>();
        }
    }
}
