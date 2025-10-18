using Microsoft.EntityFrameworkCore;
using WebNews.Data.UnitOfWork;
using WebNews.Models.Entities;
using WebNews.Services.Interfaces;

namespace WebNews.Services;

public class NewsService : INewsService
{
    private readonly IUnitOfWork _unitOfWork;

    public NewsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<News>> GetAllAsync()
    {
        return await _unitOfWork.NewsRepository.GetAllAsync();
    }

    public async Task<News?> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.NewsRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<News>> GetLatestAsync(int count)
    {
        var allNews = await GetAllAsync();
        var latestNews = allNews
            .OrderByDescending(n => n.DateTime)
            .Take(count);

        return latestNews;
    }

    public async Task CreateAsync(News news)
    {
        await _unitOfWork.NewsRepository.AddAsync(news);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(News news)
    {
        _unitOfWork.NewsRepository.Update(news);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var getNews = await GetByIdAsync(id);

        if (getNews != null)
        {
            _unitOfWork.NewsRepository.Delete(getNews);
        }

        await _unitOfWork.SaveAsync();
    }
}