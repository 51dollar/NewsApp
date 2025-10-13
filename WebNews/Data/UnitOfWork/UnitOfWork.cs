using WebNews.Data.Repository;
using WebNews.Data.Repository.Interfaces;

namespace WebNews.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private NewsRepository _newsRepository;
    private UserRepository _userRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public INewsRepository NewsRepository
    {
        get
        {
            if (_newsRepository == null)
            {
                _newsRepository = new NewsRepository(_context);
            }

            return _newsRepository;
        }
    }

    public IUserRepository UserRepository
    {
        get
        {
            if (_userRepository == null)
            {
                _userRepository = new UserRepository(_context);
            }

            return _userRepository;
        }
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}