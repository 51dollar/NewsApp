using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNews.Services;
using WebNews.Helpers;
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
            else
            {
                newsViewModel.News.ImagePath = "/Image/default.png";
            }

            newsViewModel.News.Author = "Admin";

            await _newsService.CreateNewsAsync(newsViewModel.News);
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

        var news = await _newsService.GetNewsByIdAsync(id);
        if (news == null)
        {
            return NotFound();
        }

        NewsViewModel newsViewModel = new NewsViewModel
        {
            News = news
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

        var existingNews = await _newsService.GetNewsByIdAsync(newsViewModel.News.Id);
        if (existingNews == null)
        {
            return NotFound();
        }

        existingNews.Title = newsViewModel.News.Title;
        existingNews.Subtitle = newsViewModel.News.Subtitle;
        existingNews.Content = newsViewModel.News.Content;

        if (newsViewModel.Image != null)
        {
            var newImagePath = await _fileHelper.uploadFileAsync(newsViewModel.Image);
            existingNews.ImagePath = newImagePath;
        }

        await _newsService.UpdateNewsAsync(existingNews);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        await _newsService.DeleteNewsAsync(id);
        return RedirectToAction("Index");
    }
}