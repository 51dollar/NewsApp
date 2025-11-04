namespace WebNews.Services.Interfaces.Base;

public interface IBaseCommandService<in TEntity>
    where TEntity : class
{
    Task CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
}