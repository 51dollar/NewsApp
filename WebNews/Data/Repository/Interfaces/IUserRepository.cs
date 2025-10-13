using WebNews.Models;

namespace WebNews.Data.Repository.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email);
}