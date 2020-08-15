using FluentValidation;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Api.Models
{
    public class CommentCreateRequest
    {
        public string Body { get; set; }

        public class CommentCreateRequestValidator : AbstractValidator<CommentCreateRequest>
        {
            public CommentCreateRequestValidator(ILimits limits)
            {
                RuleFor(x => x.Body)
                    .MinimumLength(limits.CommentBodyMinimumLength)
                    .WithMessage($"Comment's body must be at least {limits.CommentBodyMinimumLength} characters.");

            }
        }
    }
}
