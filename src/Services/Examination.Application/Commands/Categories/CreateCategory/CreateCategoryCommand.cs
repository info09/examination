using Examination.Shared.Categories;
using Examination.Shared.SeedWorks;
using MediatR;

namespace Examination.Application.Commands.Categories.CreateCategory
{
    public class CreateCategoryCommand : IRequest<ApiResult<CategoryDto>>
    {
        public string Name { set; get; }
        public string UrlPath { get; set; }
    }
}
