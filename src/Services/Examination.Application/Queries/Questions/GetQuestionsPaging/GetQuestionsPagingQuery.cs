using Examination.Shared.Questions;
using Examination.Shared.SeedWorks;
using MediatR;

namespace Examination.Application.Queries.Questions.GetQuestionsPaging
{
    public class GetQuestionsPagingQuery : IRequest<ApiResult<PagedList<QuestionDto>>>
    {
        public string CategoryId { get; set; }
        public string SearchKeyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
