using WebNews.Data.Repository;
using WebNews.Data.Repository.Interfaces;
using WebNews.Models;

namespace WebNews.Data.UnitOfWork;

public interface IUnitOfWork
{
    INewsRepository NewsRepository { get; }
    IUserRepository UserRepository { get; }
    Task SaveAsync();
}