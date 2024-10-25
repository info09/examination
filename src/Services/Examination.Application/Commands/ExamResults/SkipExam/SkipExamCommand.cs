using Examination.Shared.SeedWorks;
using MediatR;

namespace Examination.Application.Commands.ExamResults.SkipExam
{
    public class SkipExamCommand : IRequest<ApiResult<bool>>
    {
        public string ExamResultId { get; set; }
    }
}
