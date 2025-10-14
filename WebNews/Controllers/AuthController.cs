using Microsoft.AspNetCore.Mvc;
using WebNews.Helpers;
using WebNews.Models;
using WebNews.Models.ViewModels;
using WebNews.Services;

namespace WebNews.Controllers;

public class AuthController : Controller
{
    private readonly UserService _service;
    private readonly Hasher<User> _hasher;
    private readonly AuthHelper _authHelper;

    public AuthController(UserService service, Hasher<User> hasher, AuthHelper authHelper)
    {
        _service = service;
        _hasher = hasher;
        _authHelper = authHelper;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var userFromDb = await _service.GetUserByEmailAsync(request.Email);

        if (userFromDb != null && userFromDb.Email.Equals(request.Email))
        {
            ViewBag.Error = "Email already exists";
            return View(request);
        }

        User user = new User()
        {
            Username = request.Username,
            Email = request.Email,
            Role = RoleType.User,
        };

        user.PasswordHash = _hasher.HashPassword(
            user,
            request.Password
        );

        await _service.CreateAsync(user);
        await _authHelper.SignInUserAsync(user);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var userFromDb = await _service.GetUserByEmailAsync(request.Email);

        if (userFromDb == null)
        {
            ViewBag.Error = "User is not found";
            return View(request);
        }

        var isPasswordValid = _hasher.VerifyPassword(
            userFromDb,
            userFromDb.PasswordHash,
            request.Password);

        if (!isPasswordValid)
        {
            ViewBag.Error = "Passwords no match";
            return View(request);
        }

        await _authHelper.SignInUserAsync(userFromDb);

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _authHelper.SignOutUserAsync();
        return RedirectToAction("Index", "Home");
    }
}