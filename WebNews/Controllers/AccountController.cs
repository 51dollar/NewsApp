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
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        
        var modelUser = await _userService.GetByIdAsync(Guid.Parse(userId));
        if (modelUser == null)
            return BadRequest();
        
        return View(modelUser);
    }
}