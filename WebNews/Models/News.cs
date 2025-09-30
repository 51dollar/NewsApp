using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNews.Models;

public class News
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Title is required")]
    [MaxLength(400, ErrorMessage = "Title cannot be longer than 400 characters")]
    public string Title { get; set; }

    [MaxLength(1000, ErrorMessage = "Subtitle cannot be longer than 1000 characters")]
    public string Subtitle { get; set; }

    [Required(ErrorMessage = "Content is required")]
    [MaxLength(5000, ErrorMessage = "Content cannot exceed 5000 characters")]
    public string Content { get; set; }

    public string? Image { get; set; }

    public string Author { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime DateTime { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; }

    public User User { get; set; }

    [NotMapped]
    public DateTime PublishDateTime => DateTime.ToLocalTime();
}