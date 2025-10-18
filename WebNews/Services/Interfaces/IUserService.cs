using WebNews.Models.Entities;

namespace WebNews.Services.Interfaces;

public interface IUserService : IGenericService<User>
{
    Task<User?> GetUserByEmailAsync(string email);
}