namespace WebNews.Services.Interfaces.Base;

public interface IBaseReadService<TEntity>
    where TEntity : class
{
    Task<IReadOnlyList<TEntity>> GetLatestAsync(int count);
    Task<TEntity?> GetByIdAsync(Guid id);
}