using Examination.Shared.Categories;
using Examination.Shared.SeedWorks;
using MediatR;

namespace Examination.Application.Queries.Categories.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<ApiResult<CategoryDto>>
    {
        public string Id { get; set; }
        public GetCategoryByIdQuery(string id)
        {
            Id = id;
        }
    }
}
