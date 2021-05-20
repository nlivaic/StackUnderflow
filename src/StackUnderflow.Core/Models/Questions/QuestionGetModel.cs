using AutoMapper;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Enums;
using StackUnderflow.Core.Models.Votes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackUnderflow.Core.Models
{
    public class QuestionGetModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool HasAcceptedAnswer { get; set; }
        public DateTime CreatedOn { get; set; }
        public int VotesSum { get; set; }
        public VoteGetModel Vote { get; set; }
        public IEnumerable<TagGetModel> Tags { get; set; } = new List<TagGetModel>();

        public class QuestionGetModelProfile : Profile
        {
            public QuestionGetModelProfile()
            {
                CreateMap<Question, QuestionGetModel>()
                    .ForMember(dest => dest.Username,
                        opts => opts.MapFrom(src => src.User.Username))
                    .ForMember(dest => dest.VotesSum,
                        opts => opts.MapFrom(src => src
                            .Votes
                            .GroupBy(v => v.VoteType)
                            .Select(grp => grp
                                .Sum(g => g.VoteType == VoteTypeEnum.Upvote
                                    ? 1
                                    : -1))
                            .Sum()))
                    .ForMember(dest => dest.Vote,
                        opts => opts.MapFrom(src => src.Votes.FirstOrDefault()))
                    .ForMember(dest => dest.Tags,
                        opts => opts.MapFrom(src => src.QuestionTags.Select(qt => qt.Tag)));
            }
        }
    }
}
