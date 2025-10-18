using Microsoft.EntityFrameworkCore;
using WebNews.Data.Repository.Interfaces;
using WebNews.Models.Entities;

namespace WebNews.Data.Repository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}