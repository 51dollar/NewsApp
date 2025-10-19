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
    private readonly NewsService _newsService;
    private readonly UserService _userService;
    private readonly ImageHelper _imageHelper;

    public AdminController(NewsService newsService, ImageHelper imageHelper, UserService userService)
    {
        _newsService = newsService;
        _imageHelper = imageHelper;
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = RoleType.Admin)]
    public async Task<IActionResult> Index()
    {
        const byte countNews = 10;
        const byte countUsers = 10;

        var allNews = await _newsService.GetLatestAsync(countNews);
        var allUsers = await _userService.GetLatestAsync(countUsers);

        var model = new AdminDashboardViewModel
        {
            NewsItems = allNews,
            Users = allUsers,
        };

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = RoleType.Admin)]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = RoleType.Admin)]
    public async Task<IActionResult> Create(CreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Date in not valid";
            return View(model);
        }

        string imagePath;

        if (model.Image != null)
        {
            if (_imageHelper.ValidFileExtension(model.Image))
            {
                ModelState.AddModelError(string.Empty,
                    "Invalid file extension. Allowed format are .jpg, .jpeg, .png");
                return View(model);
            }

            imagePath = await _imageHelper.UploadFileAsync(model.Image);
        }
        else
        {
            imagePath = "/Image/default.png";
        }

        var userIdFromCookie = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var usernameFromCookie = User.FindFirstValue(ClaimTypes.Name);

        if (userIdFromCookie == null || usernameFromCookie == null)
        {
            ViewBag.Error = "User is not registered";
            return View(model);
        }

        News news = new News()
        {
            Title = model.Title,
            Subtitle = model.Subtitle,
            Content = model.Content,
            ImagePath = imagePath,
            UserId = Guid.Parse(userIdFromCookie),
            Author = usernameFromCookie
        };

        await _newsService.CreateAsync(news);
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = RoleType.Admin)]
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || id == Guid.Empty)
        {
            return NotFound("Id news is not found");
        }

        var newsFromDb = await _newsService.GetByIdAsync(id.Value);
        if (newsFromDb == null)
        {
            return NotFound("News from db is not found");
        }

        EditViewModel editViewModel = new EditViewModel
        {
            Id = newsFromDb.Id,
            Title = newsFromDb.Title,
            Subtitle = newsFromDb.Subtitle,
            Content = newsFromDb.Content,
            ImageNews = newsFromDb.ImagePath
        };

        return View(editViewModel);
    }

    [HttpPost]
    [Authorize(Roles = RoleType.Admin)]
    public async Task<IActionResult> Edit(EditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Date in not valid.";
            return View(model);
        }

        var newsFromDb = await _newsService.GetByIdAsync(model.Id);
        if (newsFromDb == null)
        {
            return NotFound("News from db is not found");
        }

        var userIdFromCookie = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var usernameFromCookie = User.FindFirstValue(ClaimTypes.Name);

        if (userIdFromCookie == null || usernameFromCookie == null)
        {
            ViewBag.Error = "User is not registered";
            return View(model);
        }

        newsFromDb.Title = model.Title;
        newsFromDb.Subtitle = model.Subtitle;
        newsFromDb.Content = model.Content;
        newsFromDb.CreatedAtUtc = DateTime.UtcNow;
        newsFromDb.UserId = Guid.Parse(userIdFromCookie);
        newsFromDb.Author = usernameFromCookie;

        if (model.InputImage != null)
        {
            if (model.ImageNews != null && model.ImageNews != "/Image/default.png") 
            {
                _imageHelper.DeleteFile(model.ImageNews);
            }

            var newUrlImage = await _imageHelper.UploadFileAsync(model.InputImage);
            newsFromDb.ImagePath = newUrlImage;
        }

        await _newsService.UpdateAsync(newsFromDb);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        await _newsService.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}