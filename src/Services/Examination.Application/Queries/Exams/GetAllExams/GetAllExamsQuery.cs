using Examination.Shared.Exams;
using Examination.Shared.SeedWorks;
using MediatR;

namespace Examination.Application.Queries.Exams.GetAllExams
{
    public class GetAllExamsQuery : IRequest<ApiResult<IEnumerable<ExamDto>>>
    {
    }
}
