using FluentValidation;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Api.Models
{
    public class UpdateCommentRequest
    {
        public string Body { get; set; }

        public class UpdateCommentRequestValidator : AbstractValidator<UpdateCommentRequest>
        {
            public UpdateCommentRequestValidator(ILimits limits)
            {
                RuleFor(x => x.Body)
                    .MinimumLength(limits.CommentBodyMinimumLength)
                    .WithMessage($"Answer's body must be at least {limits.CommentBodyMinimumLength} characters.");

            }
        }
    }
}