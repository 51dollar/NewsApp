using AutoMapper;
using WebNews.Data.UnitOfWork;
using WebNews.Helpers.Image;
using WebNews.Services.Interfaces;

namespace WebNews.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<INewsService> _newsService;

    public INewsService NewsService => _newsService.Value;
    public IUserService UserService { get; }
    public IAuthService AuthService { get; }

    public ServiceManager(IUnitOfWork uow, IMapper mapper, ImageHelper imageHelper)
    {
        _newsService = new Lazy<INewsService>(
            () => new NewsService(uow, mapper, imageHelper));
        UserService = new UserService(uow, mapper);
        AuthService = new AuthService(uow, mapper);
    }
}