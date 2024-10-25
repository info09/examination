using Examination.Shared.ExamResults;
using Examination.Shared.SeedWorks;
using MediatR;

namespace Examination.Application.Commands.ExamResults.StartExam
{
    public class StartExamCommand : IRequest<ApiResult<ExamResultDto>>
    {
        public string ExamId { get; set; }
    }
}
