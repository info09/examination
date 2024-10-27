using Examination.Shared.Exams;
using Examination.Shared.SeedWorks;
using MediatR;

namespace Examination.Application.Queries.Exams.GetExamsPaging
{
    public class GetExamsPagingQuery : IRequest<ApiResult<PagedList<ExamDto>>>
    {
        public string? CategoryId { get; set; }
        public string? SearchKeyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}