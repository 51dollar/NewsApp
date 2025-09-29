using WebNews.Data.Repository;
using WebNews.Models;

namespace WebNews.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private Repository<News> _newsRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IRepository<News> NewsRepository
    {
        get
        {
            if (_newsRepository == null)
            {
                _newsRepository = new Repository<News>(_context);
            }

            return _newsRepository;
        }
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}