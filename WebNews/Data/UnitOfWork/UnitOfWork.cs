using Microsoft.AspNetCore.Identity;
using WebNews.Data.Repository;
using WebNews.Data.Repository.Interfaces;
using WebNews.Models.Entities;

namespace WebNews.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly Lazy<NewsRepository> _newsRepository;

    public UnitOfWork(
        AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, SignInManager<User> signInManager)
    {
        _context = context;
        UserManager = userManager;
        RoleManager = roleManager;
        SignInManager = signInManager;
        _newsRepository = new Lazy<NewsRepository>(
            () => new NewsRepository(_context));
    }

    public INewsRepository NewsRepository => _newsRepository.Value;
    public UserManager<User> UserManager { get; }
    public RoleManager<IdentityRole<Guid>> RoleManager { get; }
    public SignInManager<User> SignInManager { get; }


    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}