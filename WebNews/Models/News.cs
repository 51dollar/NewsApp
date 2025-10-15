using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNews.Models;

public class News
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    public string Subtitle { get; set; }

    public string Content { get; set; }

    public string ImagePath { get; set; }

    public string? Author { get; set; }

    [DataType(DataType.Date)]
    public DateTime DateTime { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; }

    public User User { get; set; }

    [NotMapped]
    public DateTime PublishDateTime => DateTime.ToLocalTime();
}