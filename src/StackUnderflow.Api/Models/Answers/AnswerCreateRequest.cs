using AutoMapper;
using FluentValidation;
using StackUnderflow.Application.Answers.Models;
using StackUnderflow.Core.Interfaces;

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
            public AnswerCreateRequestValidator(ILimits limits)
            {
                RuleFor(x => x.Body)
                    .MinimumLength(limits.AnswerBodyMinimumLength)
                    .WithMessage($"Answer's body must be at least {limits.CommentBodyMinimumLength} characters.");
            }
        }
    }
}
