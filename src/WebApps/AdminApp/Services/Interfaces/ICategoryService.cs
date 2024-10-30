using Examination.Shared.Categories;
using Examination.Shared.SeedWorks;

namespace AdminApp.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResult<PagedList<CategoryDto>>> GetCategoriesPagingAsync(CategorySearch searchInput);
        Task<ApiResult<List<CategoryDto>>> GetAllCategories();
        Task<ApiResult<CategoryDto>> GetCategoryByIdAsync(string id);
        Task<bool> CreateAsync(CreateCategoryRequest request);
        Task<bool> UpdateAsync(UpdateCategoryRequest request);
        Task<bool> DeleteAsync(string id);
    }
}
