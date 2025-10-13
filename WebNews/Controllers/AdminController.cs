using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNews.Helpers;
using WebNews.Models;
using WebNews.Models.ViewModels;
using WebNews.Services.Interfaces;

namespace WebNews.Controllers;

//[Authorize]
public class AdminController : Controller
{
    private readonly INewsService _service;
    private readonly UploadFileToFolder _fileHelper;
    private readonly string[] _allowedExtension = { ".jpg", ".jpeg", ".png" };

    public AdminController(INewsService service, UploadFileToFolder fileHelper)
    {
        _service = service;
        _fileHelper = fileHelper;
    }

    public async Task<IActionResult> Index()
    {
        var allNews = await _service.GetLatestAsync(100);
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
            else
            {
                newsViewModel.News.ImagePath = "/Image/default.png";
            }

            newsViewModel.News.Author = "Admin";

            await _service.CreateAsync(newsViewModel.News);
            return RedirectToAction("Index");
        }

        return View(newsViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var newsFromDb = await _service.GetByIdAsync(id);
        if (newsFromDb == null)
        {
            return NotFound();
        }

        NewsViewModel newsViewModel = new NewsViewModel
        {
            News = newsFromDb
        };

        return View(newsViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(NewsViewModel newsViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(newsViewModel);
        }

        var existingNews = await _service.GetByIdAsync(newsViewModel.News.Id);
        if (existingNews == null)
        {
            return NotFound();
        }

        existingNews.Title = newsViewModel.News.Title;
        existingNews.Subtitle = newsViewModel.News.Subtitle;
        existingNews.Content = newsViewModel.News.Content;
        existingNews.DateTime = DateTime.UtcNow;

        if (newsViewModel.Image != null)
        {
            var newImagePath = await _fileHelper.uploadFileAsync(newsViewModel.Image);
            existingNews.ImagePath = newImagePath;
        }

        await _service.UpdateAsync(existingNews);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}