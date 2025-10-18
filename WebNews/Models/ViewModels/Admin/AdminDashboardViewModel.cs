using WebNews.Models.Entities;

namespace WebNews.Models.ViewModels.Admin;

public class AdminDashboardViewModel
{
    public IEnumerable<Entities.News> NewsItems { get; set; } =  [];
    public IEnumerable<User> Users { get; set; } =  [];
    
}