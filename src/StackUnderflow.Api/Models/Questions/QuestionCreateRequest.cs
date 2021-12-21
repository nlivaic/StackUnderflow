using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentValidation;
using StackUnderflow.Application.Questions.Models;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Api.Models
{
    public class QuestionCreateRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public IEnumerable<Guid> TagIds { get; set; }

        public class QuestionProfile : Profile
        {
            public QuestionProfile()
            {
                CreateMap<QuestionCreateRequest, QuestionCreateModel>();
            }
        }

        public class QuestionCreateRequestValidator : AbstractValidator<QuestionCreateRequest>
        {
            public QuestionCreateRequestValidator(ILimits limits)
            {
                RuleFor(x => x.Body)
                    .MinimumLength(limits.QuestionBodyMinimumLength)
                    .WithMessage($"Question's body must be at least {limits.QuestionBodyMinimumLength} characters.");
                RuleFor(x => x.TagIds)
                    .Must(t =>
                    {
                        var count = t.Count();
                        return count >= limits.TagMinimumCount && count <= limits.TagMaximumCount;
                    })
                    .WithMessage($"Question must be tagged with {limits.TagMinimumCount} to {limits.TagMaximumCount} tags.");
            }
        }
    }
}
