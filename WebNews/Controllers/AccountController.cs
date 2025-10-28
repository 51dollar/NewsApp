using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNews.Services;

namespace WebNews.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly UserService _userService;

    public AccountController(UserService userService)
    {
        _userService = userService;
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