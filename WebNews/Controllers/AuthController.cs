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
    private readonly JwtService _jwtService;
    private readonly Hasher<User> _hasher;

    public AuthController(UserService service, JwtService jwtService, Hasher<User> hasher)
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

        User user = new User()
        {
            Username = userViewModel.User.Username,
            Email = userViewModel.User.Email,
            PasswordHash = userViewModel.User.PasswordHash
        };

        await _service.CreateAsync(user);
        return RedirectToAction("Home");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserViewModel userViewModel)
    {
        if (ModelState.IsValid)
        {
            var userFromDb = await _service.GetUserByEmailAsync(userViewModel.User.Email);
            if (userFromDb == null)
            {
                ViewBag.Error = "Email doesn't exist";
                return View(userViewModel);
            }

            userViewModel.User.PasswordHash = _hasher.HashPassword(
                userViewModel.User,
                userViewModel.User.PasswordHash
            );

            if (userViewModel.User.PasswordHash.Equals(userFromDb.PasswordHash))
            {
                ViewBag.Error = "Passwords no match";
                return View(userViewModel);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userViewModel.User.Id.ToString()),
                new Claim(ClaimTypes.Email, userViewModel.User.Username),
                new Claim(ClaimTypes.Name, userViewModel.User.Email),
                new Claim(ClaimTypes.Role, userViewModel.User.Role),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }

        return RedirectToAction("Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Home");
    }
}