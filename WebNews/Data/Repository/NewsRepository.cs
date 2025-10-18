using WebNews.Data.Repository.Interfaces;
using WebNews.Models.Entities;

namespace WebNews.Data.Repository;

public class NewsRepository : GenericRepository<News>, INewsRepository
{
    public NewsRepository(AppDbContext context) : base(context)
    {
    }
}