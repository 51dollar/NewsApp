using WebNews.Data.Repository.Interfaces;
using WebNews.Models;

namespace WebNews.Data.Repository;

public class NewsRepository : GenericRepository<News>, INewsRepository
{
    public NewsRepository(AppDbContext context) : base(context)
    {
    }
}