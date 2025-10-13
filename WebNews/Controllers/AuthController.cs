using Microsoft.AspNetCore.Mvc;
using WebNews.Helpers;
using WebNews.Models;
using WebNews.Models.ViewModels;
using WebNews.Services;
using WebNews.Services.Interfaces;

namespace WebNews.Controllers;

public class AuthController : Controller
{
    private readonly IUserService _service;
    private readonly JwtService _jwtService;
    private readonly Hasher<User> _hasher;

    public AuthController(IUserService service, JwtService jwtService, Hasher<User> hasher)
    {
        _service = service;
        _jwtService = jwtService;
        _hasher = hasher;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserViewModel userViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(userViewModel);
        }

        var userFromDb = await _service.GetUserByEmailAsync(userViewModel.User.Email);

        if (userFromDb.Equals(userViewModel.User.Email))
        {
            ViewBag.Error = "Email already exists";
            return View(userViewModel);
        }

        userViewModel.User.PasswordHash = _hasher.HashPassword(
            userViewModel.User,
            userViewModel.User.PasswordHash
        );

        await _service.CreateAsync(userViewModel.User);
        return RedirectToAction("Home");
    }
}