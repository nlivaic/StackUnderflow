using System;
using AutoMapper;
using StackUnderflow.Application.Votes.Models;
using StackUnderflow.Core.Enums;

namespace StackUnderflow.Api.Models.Votes
{
    public class VoteGetViewModel
    {
        public Guid Id { get; set; }
        public string VoteType { get; set; }
        public Guid TargetId { get; set; }

        public class VoteGetViewModelProfile : Profile
        {
            public VoteGetViewModelProfile()
            {
                CreateMap<VoteGetModel, VoteGetViewModel>()
                    .ForMember(
                        dest => dest.VoteType,
                        opts => opts.MapFrom(src =>
                            src.VoteType == VoteTypeEnum.Upvote
                                ? VoteTypeAsText.Upvote.Text
                                : VoteTypeAsText.Downvote.Text));
            }
        }

        private class VoteTypeAsText
        {
            private VoteTypeAsText(string text)
            {
                Text = text;
            }

            public static VoteTypeAsText Upvote => new VoteTypeAsText("Upvote");
            public static VoteTypeAsText Downvote => new VoteTypeAsText("Downvote");

            public string Text { get; }
        }
    }
}
