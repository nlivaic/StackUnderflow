using FluentValidation;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Api.Models
{
    public class UpdateCommentOnQuestionRequest
    {
        public string Body { get; set; }

        public class UpdateCommentOnQuestionRequestValidator : AbstractValidator<UpdateCommentOnQuestionRequest>
        {
            public UpdateCommentOnQuestionRequestValidator(ILimits limits)
            {
                RuleFor(x => x.Body)
                    .MinimumLength(limits.CommentBodyMinimumLength)
                    .WithMessage($"Answer's body must be at least {limits.CommentBodyMinimumLength} characters.");

            }
        }
    }
}