using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_Web_News.Services;
using WebNews.Helpers;
using WebNews.Models;
using WebNews.Models.ViewModels;

namespace WebNews.Controllers;

//[Authorize]
public class AdminController : Controller
{
    private readonly INewsService _newsService;
    private readonly UploadFileToFolder _fileHelper;
    private readonly string[] _allowedExtension = { ".jpg", ".jpeg", ".png" };

    public AdminController(INewsService newsService, UploadFileToFolder fileHelper)
    {
        _newsService = newsService;
        _fileHelper = fileHelper;
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
    public async Task<IActionResult> Create(NewsViewModel newsViewModel)
    {
        if (ModelState.IsValid)
        {
            if (newsViewModel.Image != null)
            {
                var inputFileExtension =
                    Path.GetExtension(newsViewModel.Image.FileName).ToLower();
                bool isAllowed = _allowedExtension.Contains(inputFileExtension);

                if (!isAllowed)
                {
                    ModelState.AddModelError(string.Empty,
                        "Invalid file extension. Allowed format are .jpg, .jpeg, .png");
                    return View(newsViewModel);
                }

                newsViewModel.News.ImagePath = await _fileHelper.uploadFileAsync(newsViewModel.Image);
            }

            await _newsService.CreateNewsAsync(newsViewModel.News);
            return RedirectToAction("Index");
        }

        return View(newsViewModel);
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