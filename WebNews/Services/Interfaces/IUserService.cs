using WebNews.Models.Entities;
using WebNews.Models.ViewModels.Account;
using WebNews.Services.Interfaces.Base;

namespace WebNews.Services.Interfaces;

public interface IUserService : IBaseReadService<User>
{
    Task<AccountViewModel> GetAccountByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(User entity);
}