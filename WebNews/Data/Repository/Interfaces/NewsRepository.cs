using WebNews.Models;

namespace WebNews.Data.Repository.Interfaces;

public class NewsRepository : GenericRepository<News>, INewsRepository
{
    public NewsRepository(AppDbContext context) : base(context)
    {
    }
}