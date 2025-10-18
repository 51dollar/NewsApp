using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNews.Models.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime DateCreate { get; set; } = DateTime.UtcNow;
    public ICollection<News> News { get; set; } = new List<News>();
    
    [NotMapped]
    public DateTime PublishDateCreate => DateCreate.ToLocalTime();
}