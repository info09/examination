using Examination.Shared.Categories;
using Examination.Shared.SeedWorks;
using MediatR;

namespace Examination.Application.Queries.Categories.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<ApiResult<List<CategoryDto>>>
    {
    }
}
