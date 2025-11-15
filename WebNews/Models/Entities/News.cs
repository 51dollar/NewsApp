using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNews.Models.Entities;

public class News
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public string? Author { get; set; }
    public uint CountViews { get; set; } = 0;

    [DataType(DataType.Date)]
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [NotMapped]
    public DateTime PublishDateTime => CreatedAtUtc.ToLocalTime();
}