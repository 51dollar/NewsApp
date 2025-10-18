using WebNews.Data.Repository.Interfaces;

namespace WebNews.Data.UnitOfWork;

public interface IUnitOfWork
{
    INewsRepository NewsRepository { get; }
    IUserRepository UserRepository { get; }
    Task SaveAsync();
}