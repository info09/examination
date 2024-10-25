using Examination.Shared.ExamResults;
using Examination.Shared.SeedWorks;
using MediatR;

namespace Examination.Application.Queries.ExamResults.GetExamResultById
{
    public class GetExamResultByIdQuery : IRequest<ApiResult<ExamResultDto>>
    {
        public string Id { get; set; }
        public GetExamResultByIdQuery(string id)
        {
            Id = id;
        }
    }
}
