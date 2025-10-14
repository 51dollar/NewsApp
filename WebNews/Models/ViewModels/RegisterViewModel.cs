using System.ComponentModel.DataAnnotations;

namespace WebNews.Models.ViewModels;

public class RegisterViewModel
{
    [Required]
    [MaxLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
    public string Username { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "Email cannot be longer than 50 characters")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [MaxLength(50, ErrorMessage = "Password cannot be longer than 50 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}