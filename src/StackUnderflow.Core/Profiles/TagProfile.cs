using AutoMapper;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Profiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagGetModel>();
        }
    }
}
