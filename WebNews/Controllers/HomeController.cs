using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebNews.Models.ViewModels.Errors;
using WebNews.Services.Interfaces;

namespace WebNews.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IServiceManager _service;

    public HomeController(IServiceManager service, ILogger<HomeController> logger)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int countNews = 8)
    {
        var latestNew = await _service.NewsService.GetLatestAsync(countNews);
        return View(latestNew);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}