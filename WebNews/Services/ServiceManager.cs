using WebNews.Services.Interfaces;

namespace WebNews.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<INewsService> _newsService;

    public INewsService NewsService => _newsService.Value;
    public IUserService UserService { get; }
    public IAuthService AuthService { get; }

    public ServiceManager(
        IUserService userService, 
        IAuthService authService,
        INewsService newsService)
    {
        UserService = userService;
        AuthService = authService;
        _newsService = new Lazy<INewsService>(newsService);
    }
}