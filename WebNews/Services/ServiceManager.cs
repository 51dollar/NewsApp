using AutoMapper;
using WebNews.Data.UnitOfWork;
using WebNews.Helpers.Auth;
using WebNews.Helpers.Image;
using WebNews.Services.Interfaces;

namespace WebNews.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<INewsService> _newsService;
    private readonly Lazy<IUserService> _userService;

    public INewsService NewsService => _newsService.Value;
    public IUserService UserService => _userService.Value;

    public ServiceManager(
        IUnitOfWork uow,
        Hasher hasher,
        AuthService authService,
        IMapper mapper,
        ImageHelper imageHelper
    )
    {
        _newsService = new Lazy<INewsService>(() => new NewsService(uow, mapper, imageHelper));
        _userService = new Lazy<IUserService>(() => new UserService(uow, hasher, authService, mapper));
    }
}