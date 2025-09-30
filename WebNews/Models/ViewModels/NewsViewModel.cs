namespace WebNews.Models.ViewModels;

public class NewsViewModel
{
    public News News { get; set; } = new News();
    public IFormFile? Image { get; set; }
}