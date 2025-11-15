using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNews.Models.ViewModels.Account;
using WebNews.Services.Interfaces;

namespace WebNews.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IServiceManager userService)
    {
        _userService = userService.UserService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        try
        {
            var accountViewModel = await _userService.GetAccountByIdAsync(Guid.Parse(userId));
            return View(accountViewModel);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(AccountViewModel? response)
    {
        if (response == null)
            return BadRequest();
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        try
        {
            await _userService.UpdateAccountAsync(Guid.Parse(userId), response);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    public IActionResult UpdatePassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePassword(ChangePasswordViewModel? response)
    {
        if (response == null)
            return BadRequest();
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        try
        {
            await _userService.UpdatePasswordAsync(Guid.Parse(userId), response);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            ViewBag.Error = e.Message;
            return View();
        }
    }
}