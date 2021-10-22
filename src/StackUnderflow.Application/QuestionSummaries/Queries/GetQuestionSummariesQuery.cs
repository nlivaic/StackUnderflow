using MediatR;
using StackUnderflow.Application.Questions.Models;
using StackUnderflow.Application.Sorting.Models;
using StackUnderflow.Common.Paging;
using StackUnderflow.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace StackUnderflow.Application.QuestionSummaries.Queries
{
    public class GetQuestionSummariesQuery : IRequest<PagedList<QuestionSummaryGetModel>>
    {
        public QuestionQueryParameters QuestionQueryParameters { get; set; }

        private class GetQuestionSummariesQueryHandler : IRequestHandler<GetQuestionSummariesQuery, PagedList<QuestionSummaryGetModel>>
        {
            private readonly IQuestionRepository _questionRepository;

            public GetQuestionSummariesQueryHandler(
                IQuestionRepository questionRepository)
            {
                _questionRepository = questionRepository;
            }

            public async Task<PagedList<QuestionSummaryGetModel>> Handle(
                GetQuestionSummariesQuery request,
                CancellationToken cancellationToken) =>
                await _questionRepository.GetQuestionSummariesAsync(request.QuestionQueryParameters);
        }
    }
}
