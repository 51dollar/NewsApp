using Microsoft.AspNetCore.Mvc;
using WebNews.Helpers;
using WebNews.Helpers.Auth;
using WebNews.Models;
using WebNews.Models.Entities;
using WebNews.Models.ViewModels;
using WebNews.Models.ViewModels.Auth;
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
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Date in not valid";
            return View(model);
        }

        var userFromDb = await _service.GetUserByEmailAsync(model.Email);

        if (userFromDb != null && userFromDb.Email.Equals(model.Email))
        {
            ViewBag.Error = "Email already exists";
            return View(model);
        }

        User user = new User()
        {
            Username = model.Username,
            Email = model.Email,
            Role = RoleType.User,
        };

        user.PasswordHash = _hasher.HashPassword(
            user,
            model.Password
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
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Date in not valid.";
            return View(model);
        }

        var userFromDb = await _service.GetUserByEmailAsync(model.Email);
        if (userFromDb == null)
        {
            ViewBag.Error = "User is not found";
            return View(model);
        }

        var isPasswordValid = _hasher.VerifyPassword(
            userFromDb,
            userFromDb.PasswordHash,
            model.Password);

        if (!isPasswordValid)
        {
            ViewBag.Error = "Passwords no match";
            return View(model);
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