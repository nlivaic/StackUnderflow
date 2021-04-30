using AutoMapper;
using FluentValidation;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Models
{
    public class AnswerCreateRequest
    {
        public string Body { get; set; }

        public class AnswerProfile : Profile
        {
            public AnswerProfile()
            {
                CreateMap<AnswerCreateRequest, AnswerCreateModel>();
            }
        }

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
