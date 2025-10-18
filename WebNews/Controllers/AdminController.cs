using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNews.Helpers.Auth;
using WebNews.Helpers.Image;
using WebNews.Models.Entities;
using WebNews.Models.ViewModels.Admin;
using WebNews.Models.ViewModels.News;
using WebNews.Services;

namespace WebNews.Controllers;

[Authorize(Roles = RoleType.Admin)]
public class AdminController : Controller
{
    private readonly NewsService _service;
    private readonly UploadFileToFolder _fileHelper;
    private readonly string[] _allowedExtension = { ".jpg", ".jpeg", ".png" };

    public AdminController(NewsService service, UploadFileToFolder fileHelper)
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
    [Authorize]
    public async Task<IActionResult> Create(CreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        string imagePath;

        if (model.Image != null)
        {
            var inputFileExtension = Path.GetExtension(model.Image.FileName).ToLower();
            bool isAllowed = _allowedExtension.Contains(inputFileExtension);

            if (!isAllowed)
            {
                ModelState.AddModelError(string.Empty,
                    "Invalid file extension. Allowed format are .jpg, .jpeg, .png");
                return View(model);
            }

            imagePath = await _fileHelper.UploadFileAsync(model.Image);
        }
        else
        {
            imagePath = "/Image/default.png";
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.FindFirstValue(ClaimTypes.Name);

        News news = new News()
        {
            Title = model.Title,
            Subtitle = model.Subtitle,
            Content = model.Content,
            ImagePath = imagePath,
            UserId = Guid.Parse(userId),
            Author = username
        };

        await _service.CreateAsync(news);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var newsFromDb = await _service.GetByIdAsync(id);
        if (newsFromDb == null)
        {
            return NotFound();
        }

        EditViewModel editViewModel = new EditViewModel
        {
            Id = newsFromDb.Id,
            Title = newsFromDb.Title,
            Subtitle = newsFromDb.Subtitle,
            Content = newsFromDb.Content,
            Image = newsFromDb.ImagePath
        };

        return View(editViewModel);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(EditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var existingNews = await _service.GetByIdAsync(model.Id);
        if (existingNews == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.FindFirstValue(ClaimTypes.Name);
        
        existingNews.Title = model.Title;
        existingNews.Subtitle = model.Subtitle;
        existingNews.Content = model.Content;
        existingNews.DateTime = DateTime.UtcNow;
        existingNews.UserId = Guid.Parse(userId);
        existingNews.Author = username;

        if (model.ImageFile != null)
        {
            var newImagePath = await _fileHelper.UploadFileAsync(model.ImageFile);
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