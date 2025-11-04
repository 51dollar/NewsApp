using Microsoft.EntityFrameworkCore;
using WebNews.Data;

namespace WebNews.Extensions;

public static class DbExtensions
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(o =>
        {
            o.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
    }
}