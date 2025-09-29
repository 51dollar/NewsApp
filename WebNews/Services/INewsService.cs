using WebNews.Models;

namespace MVC_Web_News.Services;

public interface INewsService
{
    Task<IEnumerable<News>> GetLatestNewsAsync(int count);
    Task<IEnumerable<News>> GetAllNewsAsync();
    Task<News> GetNewsByIdAsync(Guid id);
    Task CreateNewsAsync(News news);
    Task UpdateNewsAsync(News news);
    Task DeleteNewsAsync(Guid id);
}