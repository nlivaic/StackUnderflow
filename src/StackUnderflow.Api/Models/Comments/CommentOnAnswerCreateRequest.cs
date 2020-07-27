using FluentValidation;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Api.Models
{
    public class CommentOnAnswerCreateRequest
    {
        public string Body { get; set; }

        public class CommentOnAnswerCreateRequestValidator : AbstractValidator<CommentOnAnswerCreateRequest>
        {
            public CommentOnAnswerCreateRequestValidator(ILimits limits)
            {
                RuleFor(x => x.Body)
                    .MinimumLength(limits.CommentBodyMinimumLength)
                    .WithMessage($"Answer's body must be at least {limits.CommentBodyMinimumLength} characters.");

            }
        }
    }
}
