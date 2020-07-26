using System;
using FluentValidation;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Api.Models
{
    public class CommentOnQuestionCreateRequest
    {
        public string Body { get; set; }

        public class CommentOnQuestionCreateRequestValidator : AbstractValidator<CommentOnQuestionCreateRequest>
        {
            public CommentOnQuestionCreateRequestValidator(ILimits limits)
            {
                RuleFor(x => x.Body)
                    .MinimumLength(limits.CommentBodyMinimumLength)
                    .WithMessage($"Answer's body must be at least {limits.QuestionBodyMinimumLength} characters.");

            }
        }
    }
}
