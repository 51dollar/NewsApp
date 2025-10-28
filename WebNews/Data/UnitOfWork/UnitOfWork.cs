using WebNews.Data.Repository;
using WebNews.Data.Repository.Interfaces;

namespace WebNews.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly Lazy<NewsRepository> _newsRepository;
    private readonly Lazy<UserRepository> _userRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        _newsRepository = new Lazy<NewsRepository>(
            () => new NewsRepository(_context));
        _userRepository = new Lazy<UserRepository>(
            () => new UserRepository(_context));
    }

    public INewsRepository NewsRepository => _newsRepository.Value;
    public IUserRepository UserRepository => _userRepository.Value;

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}