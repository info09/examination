using Examination.Shared.Categories;
using Examination.Shared.SeedWorks;

namespace PortalApp.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResult<List<CategoryDto>>> GetAllCategoriesAsync();
    }
}
