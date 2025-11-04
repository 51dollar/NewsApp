using Microsoft.AspNetCore.Identity;
using WebNews.Data.Repository.Interfaces;
using WebNews.Models.Entities;

namespace WebNews.Data.UnitOfWork;

public interface IUnitOfWork
{
    INewsRepository NewsRepository { get; }
    UserManager<User> UserManager { get; }
    RoleManager<IdentityRole<Guid>> RoleManager { get; }
    public SignInManager<User> SignInManager { get; }
    Task SaveAsync();
}