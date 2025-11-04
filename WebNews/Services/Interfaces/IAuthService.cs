using WebNews.Models.Entities;
using WebNews.Models.ViewModels.Auth;

namespace WebNews.Services.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterViewModel request, string role);
    Task LoginAsync(LoginViewModel request);
    Task LogoutAsync();
}