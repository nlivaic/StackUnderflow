using AutoMapper;
using FluentValidation;
using StackUnderflow.Application.Comments.Models;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Api.Models
{
    public class UpdateCommentRequest
    {
        public string Body { get; set; }

        public class CommentProfile : Profile
        {
            public CommentProfile()
            {
                CreateMap<UpdateCommentRequest, CommentEditModel>();
            }
        }

        public class UpdateCommentRequestValidator : AbstractValidator<UpdateCommentRequest>
        {
            public UpdateCommentRequestValidator(ILimits limits)
            {
                RuleFor(x => x.Body)
                    .MinimumLength(limits.CommentBodyMinimumLength)
                    .WithMessage($"Comment's body must be at least {limits.CommentBodyMinimumLength} characters.");
            }
        }
    }
}