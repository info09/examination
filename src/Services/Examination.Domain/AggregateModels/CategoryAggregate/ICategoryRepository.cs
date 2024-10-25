using Examination.Domain.SeedWork;
using Examination.Shared.SeedWorks;

namespace Examination.Domain.AggregateModels.CategoryAggregate
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        Task<PagedList<Category>> GetCategoriesPagingAsync(string searchKeyword, int pageIndex, int pageSize);

        Task<Category> GetCategoriesByIdAsync(string id);

        Task<Category> GetCategoriesByNameAsync(string name);

        Task<List<Category>> GetAllCategoriesAsync();
    }
}
