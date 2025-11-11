using WebNews.Services.Interfaces;

namespace WebNews.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<INewsService> _newsService;
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<IAuthService> _authService;
    private readonly Lazy<IRoleService> _roleService;

    public INewsService NewsService => _newsService.Value;
    public IUserService UserService => _userService.Value;
    public IAuthService AuthService => _authService.Value;
    public IRoleService RoleService => _roleService.Value;

    public ServiceManager(IServiceProvider serviceProvider)
    {
        _newsService = new Lazy<INewsService>(serviceProvider.GetRequiredService<INewsService>);
        _userService = new Lazy<IUserService>(serviceProvider.GetRequiredService<IUserService>);
        _authService = new Lazy<IAuthService>(serviceProvider.GetRequiredService<IAuthService>);
        _roleService = new Lazy<IRoleService>(serviceProvider.GetRequiredService<IRoleService>);
    }
}