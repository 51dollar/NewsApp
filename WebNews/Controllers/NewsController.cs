using Microsoft.AspNetCore.Mvc;
using WebNews.Models;
using WebNews.Services;
using WebNews.Services.Interfaces;


namespace WebNews.Controllers;

public class NewsController : Controller
{
    private readonly IGenericService<News> _genericService;

    public NewsController(IGenericService<News> genericService)
    {
        _genericService = genericService;
    }

    public async Task<IActionResult> Index()
    {
        var allNews = await _genericService.GetAllAsync();
        return View(allNews);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var news = await _genericService.GetByIdAsync(id);

        if (news == null)
        {
            return NotFound();
        }

        return View(news);
    }
}