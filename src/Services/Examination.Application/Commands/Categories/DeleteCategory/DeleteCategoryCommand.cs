using Examination.Shared.SeedWorks;
using MediatR;

namespace Examination.Application.Commands.Categories.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<ApiResult<bool>>
    {
        public string Id { get; set; }
        public DeleteCategoryCommand(string id)
        {
            Id = id;
        }
    }
}
