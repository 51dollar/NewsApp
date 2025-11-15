using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebNews.Models.Entities;

public class User : IdentityUser<Guid>
{
    [DataType(DataType.Date)]
    public DateTime DateCreate { get; set; } = DateTime.UtcNow;
    public ICollection<News> News { get; set; } = new List<News>();
}