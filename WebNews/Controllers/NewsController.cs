using Microsoft.AspNetCore.Mvc;
using MVC_Web_News.Services;


namespace WebNews.Controllers;

public class NewsController : Controller
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    public async Task<IActionResult> Index()
    {
        var allNews = await _newsService.GetAllNewsAsync();
        return View(allNews);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var news = await _newsService.GetNewsByIdAsync(id);

        if (news == null)
        {
            return NotFound();
        }

        return View(news);
    }
}