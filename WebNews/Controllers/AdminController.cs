using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_Web_News.Services;
using WebNews.Models;

namespace WebNews.Controllers;

//[Authorize]
public class AdminController : Controller
{
    private readonly INewsService _newsService;

    public AdminController(INewsService newsService)
    {
        _newsService = newsService;
    }

    public async Task<IActionResult> Index()
    {
        var allNews = await _newsService.GetAllNewsAsync();
        return View(allNews);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(News news)
    {
        if (ModelState.IsValid)
        {
            return View(news);
        }

        await _newsService.CreateNewsAsync(news);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var news = await _newsService.GetNewsByIdAsync(id);
        if (news == null)
        {
            return NotFound();
        }

        return View(news);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(News news)
    {
        if (!ModelState.IsValid)
        {
            return View(news);
        }

        await _newsService.UpdateNewsAsync(news);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        await _newsService.DeleteNewsAsync(id);
        return RedirectToAction("Index");
    }
}