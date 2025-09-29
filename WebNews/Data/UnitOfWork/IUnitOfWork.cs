using WebNews.Data.Repository;
using WebNews.Models;

namespace WebNews.Data.UnitOfWork;

public interface IUnitOfWork
{
    IRepository<News> NewsRepository { get; }
    Task SaveAsync();
}