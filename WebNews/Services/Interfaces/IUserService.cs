using WebNews.Models.Entities;
using WebNews.Models.ViewModels.Auth;

namespace WebNews.Services.Interfaces;

public interface IUserService : IGenericService<User>
{
    Task RegisterAsync(RegisterViewModel entity);
    Task LoginAsync(LoginViewModel entity);
    Task LogoutAsync();
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> IsUserExistsByEmailAsync(string email);
}