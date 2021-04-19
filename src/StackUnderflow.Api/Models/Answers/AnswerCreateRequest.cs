using FluentValidation;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Api.Models
{
    public class AnswerCreateRequest
    {
        public string Body { get; set; }

        public class AnswerCreateRequestValidator : AbstractValidator<AnswerCreateRequest>
        {
            public AnswerCreateRequestValidator(BaseLimits limits)
            {
                RuleFor(x => x.Body)
                    .MinimumLength(limits.AnswerBodyMinimumLength)
                    .WithMessage($"Answer's body must be at least {limits.CommentBodyMinimumLength} characters.");
            }
        }
    }
}
