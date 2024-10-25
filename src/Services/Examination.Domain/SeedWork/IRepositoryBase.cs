namespace Examination.Domain.SeedWork
{
    public interface IRepositoryBase<T> where T : IAggregateRoot
    {
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
