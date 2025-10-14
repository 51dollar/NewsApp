using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    public async Task<IActionResult> Register(UserViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var userFromDb = await _service.GetUserByEmailAsync(request.User.Email);

        if (userFromDb != null && userFromDb.Equals(request.User.Email))
        {
            ViewBag.Error = "Email already exists";
            return View(request);
        }

        request.User.PasswordHash = _hasher.HashPassword(
            request.User,
            request.User.PasswordHash
        );

        User user = new User()
        {
            Username = request.User.Username,
            Email = request.User.Email,
            PasswordHash = request.User.PasswordHash
        };

        await _service.CreateAsync(user);
        await _authHelper.SignInUserAsync(user);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserViewModel request)
    {
        if (ModelState.IsValid)
        {
            var userFromDb = await _service.GetUserByEmailAsync(request.User.Email);
            if (userFromDb == null)
            {
                ViewBag.Error = "Email doesn't exist";
                return View(request);
            }

            var isPasswordValid = _hasher.VerifyPassword(
                request.User,
                userFromDb.PasswordHash,
                request.User.PasswordHash);

            if (!isPasswordValid)
            {
                ViewBag.Error = "Passwords no match";
                return View(request);
            }

            User user = new User()
            {
                Username = request.User.Username,
                Email = request.User.Email,
                PasswordHash = request.User.PasswordHash
            };

            await _authHelper.SignInUserAsync(user);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _authHelper.SignOutUserAsync();
        return RedirectToAction("Index", "Home");
    }
}