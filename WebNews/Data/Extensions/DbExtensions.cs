using Microsoft.EntityFrameworkCore;

namespace WebNews.Data.Extensions;

public static class DbExtensions
{
    public static void AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(o =>
        {
            o.UseSqlServer("Server=THINKBOOK;Database=WebNewsDb;Trusted_Connection=True;TrustServerCertificate=True;");
        });
    }
}