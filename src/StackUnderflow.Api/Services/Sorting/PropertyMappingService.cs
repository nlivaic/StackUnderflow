using System;
using System.Collections.Generic;
using System.Linq;
using StackUnderflow.Api.Models;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.API.Services.Sorting
{
    public interface IPropertyMappingService
    {
        PropertyMappingValue GetMapping<TSource, TTarget>(string sourcePropertyName);
        IEnumerable<PropertyMappingValue> GetMappings<TSource, TTarget>(params string[] sourcePropertyNames);
    }

    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly IEnumerable<IPropertyMapping> _propertyMappings =
            new List<IPropertyMapping>
            {
                new PropertyMapping<QuestionSummaryGetViewModel, QuestionSummaryGetModel>()
                    .Add(nameof(QuestionSummaryGetViewModel.Username), $"{nameof(User)}.{nameof(User.Username)}")
                    .Add(nameof(QuestionSummaryGetViewModel.HasAcceptedAnswer), nameof(QuestionSummaryGetModel.HasAcceptedAnswer))
                    .Add(nameof(QuestionSummaryGetViewModel.CreatedOn), nameof(QuestionSummaryGetModel.CreatedOn))
                    .Add(nameof(QuestionSummaryGetViewModel.VotesSum), nameof(QuestionSummaryGetModel.VotesSum)),
                new PropertyMapping<AnswerGetViewModel, AnswerGetModel>()
                    .Add(nameof(AnswerGetViewModel.CreatedOn), nameof(AnswerGetModel.CreatedOn))
                    .Add(nameof(AnswerGetViewModel.VotesSum), nameof(AnswerGetModel.VotesSum))
            };

        public PropertyMappingValue GetMapping<TSource, TTarget>(string sourcePropertyName)
        {
            var propertyMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TTarget>>()
                .FirstOrDefault()
                ?? throw new ArgumentException($"Unknown property mapping types: {typeof(TSource)}, {typeof(TTarget)}.");
            return propertyMapping.GetMapping(sourcePropertyName);
        }

        public IEnumerable<PropertyMappingValue> GetMappings<TSource, TTarget>(params string[] sourcePropertyNames)
        {
            var propertyMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TTarget>>()
                .FirstOrDefault()
                ?? throw new ArgumentException($"Unknown property mapping types: {typeof(TSource)}, {typeof(TTarget)}.");
            return propertyMapping.GetMappings(sourcePropertyNames);
        }
    }
}
