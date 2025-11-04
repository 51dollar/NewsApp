namespace WebNews.Services.Interfaces;

public interface IServiceManager
{
    INewsService NewsService { get; }
    IUserService UserService { get; }
    IAuthService AuthService { get; }
}