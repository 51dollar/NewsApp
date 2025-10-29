using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}