using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNews.Models;

public class News
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [MinLength(1, ErrorMessage = "Title cannot be empty")]
    [MaxLength(400, ErrorMessage = "Title cannot be longer than 400 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Subtitle is required")]
    [MinLength(1, ErrorMessage = "Subtitle cannot be empty")]
    [MaxLength(1000, ErrorMessage = "Subtitle cannot be longer than 1000 characters")]
    public string Subtitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Content is required")]
    [MinLength(1, ErrorMessage = "Content cannot be empty")]
    [MaxLength(5000, ErrorMessage = "Content cannot exceed 5000 characters")]
    public string Content { get; set; } = string.Empty;

    [Column(TypeName = "varbinary(max)")]
    public byte[]? Image { get; set; }

    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime DateTime { get; set; } = DateTime.UtcNow;

    [ForeignKey("User")]
    public Guid UserId { get; set; }

    public User User { get; set; }

    [NotMapped]
    public DateTime PublishDateTime => DateTime.ToLocalTime();
}