using System;
using System.Linq;
using System.Collections.Generic;
using FluentValidation;
using StackUnderflow.Core.Interfaces;
using AutoMapper;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Models
{
    public class QuestionUpdateRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public IEnumerable<Guid> TagIds { get; set; }

        public class QuestionProfile : Profile
        {
            public QuestionProfile()
            {
                CreateMap<QuestionUpdateRequest, QuestionEditModel>();
            }
        }

        public class QuestionUpdateRequestValidator : AbstractValidator<QuestionUpdateRequest>
        {
            public QuestionUpdateRequestValidator(BaseLimits limits)
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