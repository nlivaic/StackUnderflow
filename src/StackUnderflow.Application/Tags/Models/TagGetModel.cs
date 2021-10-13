using AutoMapper;
using StackUnderflow.Core.Entities;
using System;

namespace StackUnderflow.Application.Tags.Models
{
    public class TagGetModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public class TagProfile : Profile
        {
            public TagProfile()
            {
                CreateMap<Tag, TagGetModel>();
            }
        }
    }
}
