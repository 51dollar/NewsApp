using WebNews.Models.DTOs;

namespace WebNews.Models.ViewModels.Admin;

public class AdminDashboardViewModel
{
    public IEnumerable<Entities.News> NewsItems { get; set; } =  [];
    public IEnumerable<UserDto> Users { get; set; } =  [];
}