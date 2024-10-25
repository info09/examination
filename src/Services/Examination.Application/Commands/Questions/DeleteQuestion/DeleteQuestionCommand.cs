using Examination.Shared.SeedWorks;
using MediatR;

namespace Examination.Application.Commands.Questions.DeleteQuestion
{
    public class DeleteQuestionCommand : IRequest<ApiResult<bool>>
    {
        public DeleteQuestionCommand(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
    }
}
