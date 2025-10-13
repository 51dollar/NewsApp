using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebNews.Services;
using WebNews.Models;
using WebNews.Services.Interfaces;

namespace WebNews.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IGenericService<News> _genericService;

    public HomeController(IGenericService<News> genericService, ILogger<HomeController> logger)
    {
        _logger = logger;
        _genericService = genericService;
    }

    public async Task<IActionResult> Index()
    {
        var latestNew = await _genericService.GetLatestAsync(5);
        return View(latestNew);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}