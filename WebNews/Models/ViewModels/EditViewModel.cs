using System.ComponentModel.DataAnnotations;

namespace WebNews.Models.ViewModels;

public class EditViewModel : CreateViewModel
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(400, ErrorMessage = "Title cannot be longer than 400 characters")]
    public string Title { get; set; }

    [MaxLength(1000, ErrorMessage = "Subtitle cannot be longer than 1000 characters")]
    public string Subtitle { get; set; }

    [Required(ErrorMessage = "Content is required")]
    [MaxLength(5000, ErrorMessage = "Content cannot exceed 5000 characters")]
    public string Content { get; set; }

    public string? Image { get; set; }
    
    public IFormFile? ImageFile { get; set; }
}