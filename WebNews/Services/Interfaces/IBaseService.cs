namespace WebNews.Services.Interfaces;

public interface IBaseService<T>
{
    IQueryable<T> GetAll();
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetLatestAsync(int count);
    Task<T?> GetByIdAsync(Guid id);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}