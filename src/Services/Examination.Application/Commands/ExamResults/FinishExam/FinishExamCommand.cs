using Examination.Shared.ExamResults;
using Examination.Shared.SeedWorks;
using MediatR;

namespace Examination.Application.Commands.ExamResults.FinishExam
{
    public class FinishExamCommand : IRequest<ApiResult<ExamResultDto>>
    {
        public string ExamResultId { get; set; }
    }
}