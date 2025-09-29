using WebNews.Data.UnitOfWork;
using WebNews.Models;

namespace MVC_Web_News.Services;

public class NewsService : INewsService
{
    private readonly IUnitOfWork _unitOfWork;

    public NewsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<News>> GetAllNewsAsync()
    {
        return await _unitOfWork.NewsRepository.GetAllAsync();
    }

    public async Task<News> GetNewsByIdAsync(Guid id)
    {
        return await _unitOfWork.NewsRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<News>> GetLatestNewsAsync(int count)
    {
        var allNews = await GetAllNewsAsync();
        var latestNews = allNews
            .OrderByDescending(n => n.DateTime)
            .Take(count);

        return latestNews;
    }

    public async Task CreateNewsAsync(News news)
    {
        await _unitOfWork.NewsRepository.AddAsync(news);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateNewsAsync(News news)
    {
        _unitOfWork.NewsRepository.Update(news);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteNewsAsync(Guid id)
    {
        var getNews = await GetNewsByIdAsync(id);

        if (getNews != null)
        {
            _unitOfWork.NewsRepository.Delete(getNews);
        }

        await _unitOfWork.SaveAsync();
    }
}