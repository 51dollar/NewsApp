using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebNews.Helpers;

namespace WebNews.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Username is required")]
    [MaxLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [MaxLength(50, ErrorMessage = "Email cannot be longer than 50 characters")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [MaxLength(50, ErrorMessage = "Password cannot be longer than 50 characters")]
    [DataType(DataType.Password)]
    public string PasswordHash { get; set; }

    public string Role { get; set; } = RoleType.User;

    [DataType(DataType.Date)]
    public DateTime DateCreate { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public DateTime PublishDateCreate => DateCreate.ToLocalTime();

    public ICollection<News> News { get; set; } = new List<News>();
}