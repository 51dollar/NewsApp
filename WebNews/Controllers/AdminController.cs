using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNews.Models.Entities;
using WebNews.Models.ViewModels.Admin;
using WebNews.Models.ViewModels.News;
using WebNews.Services.Interfaces;

namespace WebNews.Controllers;

[Authorize(Roles = RoleType.Admin)]
public class AdminController : Controller
{
    private readonly INewsService _newsService;
    private readonly IUserService _userService;

    public AdminController(IServiceManager newsService, IServiceManager userService)
    {
        _newsService = newsService.NewsService;
        _userService = userService.UserService;
    }

    [HttpGet]
    [Authorize(Roles = RoleType.Admin)]
    public async Task<IActionResult> Index(int countNews = 10, int countUsers = 10)
    {
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
    [ValidateAntiForgeryToken]
    [Authorize(Roles = RoleType.Admin)]
    public async Task<IActionResult> Create(CreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Date in not valid";
            return View(model);
        }
        
        var userIdFromCookie = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var usernameFromCookie = User.FindFirstValue(ClaimTypes.Name);

        if (string.IsNullOrEmpty(userIdFromCookie) || string.IsNullOrEmpty(usernameFromCookie))
        {
            ViewBag.Error = "User is not registered";
            return View(model);
        }

        try
        {
            await _newsService.CreateNewsAsync(model, userIdFromCookie, usernameFromCookie);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(model);
        }
    }

    [HttpGet]
    [Authorize(Roles = RoleType.Admin)]
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (!id.HasValue || id.Value == Guid.Empty)
        {
            return NotFound("Id news is not found");
        }

        try
        {
            var viewModel = await _newsService.ReturnViewModelAsync(id.Value);
            return View(viewModel);
        }
        catch
        {
            return NotFound("Id not found");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = RoleType.Admin)]
    public async Task<IActionResult> Edit(EditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Date in not valid.";
            return View(model);
        }

        var userIdFromCookie = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var usernameFromCookie = User.FindFirstValue(ClaimTypes.Name);

        if (string.IsNullOrEmpty(userIdFromCookie) || string.IsNullOrEmpty(usernameFromCookie))
        {
            ViewBag.Error = "User is not registered";
            return View(model);
        }

        await _newsService.UpdateNewsAsync(model, userIdFromCookie, usernameFromCookie);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = RoleType.Admin)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _newsService.DeleteAsync(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = RoleType.Admin)]
    public async Task<IActionResult> AllNews(int countNews = 100)
    {
        var listNewsDb = await _newsService.GetLatestAsync(countNews);
        return View(listNewsDb);
    }
    
    [HttpGet]
    [Authorize(Roles = RoleType.Admin)]
    public async Task<IActionResult> AllUsers(int countUsers = 100)
    {
        var listUsersDb = await _userService.GetLatestAsync(countUsers);
        return View(listUsersDb);
    }
}