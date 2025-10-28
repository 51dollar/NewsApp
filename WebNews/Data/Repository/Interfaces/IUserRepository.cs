using WebNews.Models.Entities;

namespace WebNews.Data.Repository.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
}