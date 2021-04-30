using AutoMapper;
using FluentValidation;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Models
{
    public class CommentCreateRequest
    {
        public string Body { get; set; }

        public class CommentProfile : Profile
        {
            public CommentProfile()
            {
                CreateMap<CommentCreateRequest, CommentOnQuestionCreateModel>();
                CreateMap<CommentCreateRequest, CommentOnAnswerCreateModel>();
            }
        }

        public class CommentCreateRequestValidator : AbstractValidator<CommentCreateRequest>
        {
            public CommentCreateRequestValidator(BaseLimits limits)
            {
                RuleFor(x => x.Body)
                    .MinimumLength(limits.CommentBodyMinimumLength)
                    .WithMessage($"Comment's body must be at least {limits.CommentBodyMinimumLength} characters.");

            }
        }
    }
}
