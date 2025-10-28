using Microsoft.AspNetCore.Mvc;
using WebNews.Services.Interfaces;

namespace WebNews.Controllers;

public class NewsController : Controller
{
    private readonly INewsService _service;

    public NewsController(IServiceManager service)
    {
        _service = service.NewsService;
    }

    public async Task<IActionResult> Index()
    {
        var allNews = await _service.GetAllAsync();
        return View(allNews);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || id == Guid.Empty)
        {
            return NotFound("Id news is not found");
        }

        var news = await _service.GetByIdAsync(id.Value);
        if (news == null)
        {
            return NotFound("News from db is not found");
        }

        return View(news);
    }
}