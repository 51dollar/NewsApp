using System.ComponentModel.DataAnnotations;

namespace WebNews.Models.ViewModels.Account;

public class AccountViewModel
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [StringLength(50, ErrorMessage = "Email cannot be longer than 50 characters")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;
    public DateTime DateCreate { get; set; } = DateTime.UtcNow;
    [Display(Name = "Date Create")]
    public DateTime PublishDateCreate => DateCreate.ToLocalTime();
}