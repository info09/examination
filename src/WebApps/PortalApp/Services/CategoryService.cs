using Examination.Shared.Categories;
using Examination.Shared.SeedWorks;
using PortalApp.Services.Interfaces;

namespace PortalApp.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, httpContextAccessor)
        {
        }

        public async Task<ApiResult<List<CategoryDto>>> GetAllCategoriesAsync()
        {
            var result = await GetAsync<List<CategoryDto>>("/api/Categories", true);
            return result;
        }
    }
}
