namespace WebNews.Services.Interfaces;

public interface IGenericService<T>
{
    Task<IEnumerable<T>> GetLatestAsync(int count);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}