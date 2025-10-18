using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebNews.Helpers;

namespace WebNews.Models.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Username { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string Role { get; set; }

    [DataType(DataType.Date)]
    public DateTime DateCreate { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public DateTime PublishDateCreate => DateCreate.ToLocalTime();

    public ICollection<News> News { get; set; } = new List<News>();
}