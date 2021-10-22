using System;
using AutoMapper;
using StackUnderflow.Application.Tags.Models;

namespace StackUnderflow.Api.Models
{
    public class TagGetViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public class TagProfile : Profile
        {
            public TagProfile()
            {
                CreateMap<TagGetModel, TagGetViewModel>();
            }
        }
    }
}
