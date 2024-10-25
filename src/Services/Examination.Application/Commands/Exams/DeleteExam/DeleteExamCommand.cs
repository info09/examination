using Examination.Shared.SeedWorks;
using MediatR;

namespace Examination.Application.Commands.Exams.DeleteExam
{
    public class DeleteExamCommand : IRequest<ApiResult<bool>>
    {
        public DeleteExamCommand(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
    }
}
