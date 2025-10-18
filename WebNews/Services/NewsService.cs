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
    public IQueryable<News> GetAll()
    {
        return _unitOfWork.NewsRepository.GetAll();
    }

    public async Task<IEnumerable<News>> GetAllAsync()
    {
        return await _unitOfWork.NewsRepository.GetAllAsync();
    }

    public async Task<News?> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.NewsRepository.GetByIdAsync(id);
    }
    
    public async Task<IEnumerable<News>> GetLatestAsync(byte count)
    {
        return await GetAll()
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(count)
            .ToListAsync();
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