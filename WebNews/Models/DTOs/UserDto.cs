namespace WebNews.Models.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<string> Role { get; set; }
    public DateTime DateCreate { get; set; }
    
    public DateTime PublishDateTime => DateCreate.ToLocalTime();
    public string RolesString => string.Join(", ", Role);
}