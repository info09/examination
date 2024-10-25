using Examination.Shared.ExamResults;
using Examination.Shared.SeedWorks;
using MediatR;

namespace Examination.Application.Queries.ExamResults.GetExamResultsByUserIdPaging
{
    public class GetExamResultsByUserIdPagingQuery : IRequest<ApiResult<PagedList<ExamResultDto>>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
