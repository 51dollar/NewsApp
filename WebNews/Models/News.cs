namespace WebNews.Models;

public class News
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public byte[] Image { get; set; }
    public string Subtitle { get; set; }
    public string Content { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
}