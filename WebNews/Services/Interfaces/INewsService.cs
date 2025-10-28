using WebNews.Models.Entities;
using WebNews.Models.ViewModels.News;

namespace WebNews.Services.Interfaces;

public interface INewsService : IBaseService<News>
{
    Task CreateNewsAsync(CreateViewModel model, string userId, string username);
    Task<EditViewModel?> ReturnViewModelAsync(Guid id);
    Task UpdateNewsAsync(EditViewModel model, string userId, string username);
}