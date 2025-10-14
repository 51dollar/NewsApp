using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebNews.Services;
using WebNews.Models;

namespace WebNews.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly NewsService _service;

    public HomeController(NewsService service, ILogger<HomeController> logger)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var latestNew = await _service.GetLatestAsync(5);
        return View(latestNew);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}