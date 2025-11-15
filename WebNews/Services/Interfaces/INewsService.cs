using WebNews.Models.Entities;
using WebNews.Models.ViewModels.News;
using WebNews.Services.Interfaces.Base;

namespace WebNews.Services.Interfaces;

public interface INewsService : IBaseReadService<News>, IBaseCommandService<News>
{
    Task CreateNewsAsync(CreateViewModel model, string userId, string username);
    Task<EditViewModel?> ReturnViewModelAsync(Guid id);
    Task UpdateNewsAsync(EditViewModel model, string userId, string username);
    Task RegisterViewAsync(Guid newsId, HttpRequest request, HttpResponse response);
}