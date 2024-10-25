using Examination.Shared.Exams;
using Examination.Shared.SeedWorks;
using MediatR;

namespace Examination.Application.Queries.Exams.GetExamById
{
    public class GetExamByIdQuery : IRequest<ApiResult<ExamDto>>
    {
        public string Id { get; set; }
        public GetExamByIdQuery(string id)
        {
            Id = id;
        }
    }
}
