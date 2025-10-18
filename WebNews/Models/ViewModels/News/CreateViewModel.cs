using System.ComponentModel.DataAnnotations;

namespace WebNews.Models.ViewModels.News;

public class CreateViewModel
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(400, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 400 characters")]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Subtitle cannot be longer than 1000 characters")]
    [Display(Name = "Subtitle")]
    public string? Subtitle { get; set; }

    [Required(ErrorMessage = "Content is required")]
    [StringLength(5000, MinimumLength = 20, ErrorMessage = "Content must be between 20 and 5000 characters")]
    [Display(Name = "Article Content")]
    [DataType(DataType.MultilineText)]
    public string Content { get; set; } = string.Empty;

    [Display(Name = "Upload Image")]
    [DataType(DataType.Upload)]
    public IFormFile? Image { get; set; }
}