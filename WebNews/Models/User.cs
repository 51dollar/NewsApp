using System.ComponentModel.DataAnnotations;

namespace WebNews.Models;

public class User
{
    public Guid Id { get; set; }

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
    public string Password { get; set; }

    public ICollection<News> News { get; set; } = new List<News>();
}