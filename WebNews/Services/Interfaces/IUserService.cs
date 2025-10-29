using WebNews.Models.Entities;
using WebNews.Models.ViewModels.Account;
using WebNews.Models.ViewModels.Auth;

namespace WebNews.Services.Interfaces;

public interface IUserService : IBaseService<User>
{
    Task RegisterAsync(RegisterViewModel entity);
    Task LoginAsync(LoginViewModel entity);
    Task LogoutAsync();
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> IsUserExistsByEmailAsync(string email);
    Task<AccountViewModel> GetAccountByIdAsync(Guid id);
}