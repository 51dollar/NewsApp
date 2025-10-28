using WebNews.Data.Repository.Interfaces;
using WebNews.Models.Entities;

namespace WebNews.Data.Repository;

public class NewsRepository : BaseRepository<News>, INewsRepository
{
    public NewsRepository(AppDbContext context) : base(context)
    {
    }
}